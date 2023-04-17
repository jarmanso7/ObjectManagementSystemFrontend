using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Microsoft.JSInterop;
using ObjectManagementSystemFrontend.Models;
using ObjectManagementSystemFrontend.Services;
using ObjectManagementSystemFrontend.Services.Events;

namespace ObjectManagementSystemFrontend.Components
{
    /// <summary>
    /// Displays a visual representation of Objects and Relationships as a graph of nodes connected by links.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Components.ComponentBase" />
    /// <seealso cref="System.IDisposable" />
    public partial class GraphComponent : IDisposable
    {
        // Used to position nodes randomly accross the canvas.
        private Random random = new Random();

        private Diagram Diagram;


		protected override async Task OnInitializedAsync()
		{
			await base.OnInitializedAsync();

            StateManagerService.SelectedObjectChanged += OnSelectedObjectChanged;

            StateManagerService.GeneralObjectsChanged += OnGeneralObjectsChanged;
            StateManagerService.RelationshipsChanged += OnRelationshipsChanged;

            StateManagerService.ObjectItemPropertyChanged += OnObjectItemPropertyChanged;
            StateManagerService.RelationshipItemPropertyChanged += OnRelationshipItemPropertyChanged;

            StateManagerService.Reload += OnReloadRequest;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            // Import the custom JavaScript script to handle visibility of nodes via JSInterop
            var module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./Components/GraphComponent.razor.js");
        }

		protected override void OnInitialized()
        {
            base.OnInitialized();

            var options = new DiagramOptions
            {
                DeleteKey = null,
                DefaultNodeComponent = null,
                AllowMultiSelection = false,
				AllowPanning = false,
                Links = new DiagramLinkOptions
                {
                    // Options related to links
                },
                Zoom = new DiagramZoomOptions
                {
					Enabled = false,
                },
            };

			Diagram = new Diagram(options);

			Diagram.SetZoom(0.5);
        }

        /// <summary>
        /// Handles visualization of each element in the graph according to the selected object by the user.
        /// </summary>
        /// <param name="src">The source.</param>
        /// <param name="args">The <see cref="StateChangedEventArgs{GeneralObject}"/> instance containing the event data.</param>
        private async void OnSelectedObjectChanged(object src, StateChangedEventArgs<GeneralObject> args)
        {
            await RefreshElementsVisibility();
        }

        private async Task RefreshElementsVisibility()
        {
            if (StateManagerService.SelectedObject == null)
            {
                await JSRuntime.InvokeVoidAsync("showAllNodes");
                await JSRuntime.InvokeVoidAsync("showAllLinks");
                return;
            }

            var selectedNode = Diagram.Nodes.FirstOrDefault(n => n.Id == StateManagerService.SelectedObject.Id);

            if (selectedNode == null)
            {
                await JSRuntime.InvokeVoidAsync("showAllNodes");
                await JSRuntime.InvokeVoidAsync("showAllLinks");
                return;
            }

            selectedNode.Title = StateManagerService.SelectedObject.Name;

            await ShowOnlyRelatedObjects(StateManagerService.SelectedObject.Id);
        }

        /// <summary>
        /// Shows the objects and relationships associated to the selected object
        /// </summary>
        private async Task ShowOnlyRelatedObjects(string selectedObjectId)
        {
            await JSRuntime.InvokeVoidAsync("hideAllNodes");
            await JSRuntime.InvokeVoidAsync("hideAllLinks");

            // Get the ids of all the objects related to the selectedObject
            var objectRelationships = StateManagerService.Relationships.Where(r => r.From.Id == selectedObjectId || r.To.Id == selectedObjectId);
      
            var selectedObjectRelationshipsIds = objectRelationships.Select(r => r.Id);
            var relatedObjectsIds = objectRelationships.Select(r => r.From.Id).Concat(objectRelationships.Select(r => r.To.Id)).Distinct();

            //show only related links
            await JSRuntime.InvokeVoidAsync("showLinks", new object[] { selectedObjectRelationshipsIds.ToArray() });

            //show only related nodes
            await JSRuntime.InvokeVoidAsync("showNodes", new object[] { relatedObjectsIds.ToArray() });
        }

        private void OnReloadRequest(object? sender, EventArgs e)
        {
            Diagram.Refresh();
            this.StateHasChanged();
        }

        /// <summary>
        /// Update Node title if the object's name changes
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="StateChangedEventArgs{GeneralObject}"/> instance containing the event data.</param>
        private void OnObjectItemPropertyChanged(object? sender, StateChangedEventArgs<GeneralObject> e)
        {
            Diagram.Nodes.FirstOrDefault(n => n.Id == e.Item.Id).Title = e.Item.Name;

            Diagram.Refresh();
        }
        private async void OnRelationshipItemPropertyChanged(object? sender, StateChangedEventArgs<Relationship> e)
        {
        }

        private async void OnRelationshipsChanged(object? sender, StateChangedEventArgs<Relationship> e)
        {
            switch (e.Action)
            {
                case StateChangeActionEnum.Add:

                    Diagram.Links.Add(new LinkModel(e.Item.Id,Diagram.Nodes.First(n => n.Id == e.Item.From.Id), Diagram.Nodes.First(n => n.Id == e.Item.To.Id))
                    {
                        SourceMarker = LinkMarker.Arrow,
                        TargetMarker = LinkMarker.Arrow
                    });

                    break;

                case StateChangeActionEnum.Remove:
                    Diagram.Links.Remove(Diagram.Links.First(n => n.Id == e.Item.Id));
                    break;
            }

            await RefreshElementsVisibility();
        }

        private async void OnGeneralObjectsChanged(object? sender, StateChangedEventArgs<GeneralObject> e)
        {
            switch (e.Action)
            {
                case StateChangeActionEnum.Add:
                    var node = new NodeModel(e.Item.Id, GetRandomPointWithinGraphCanvas(), RenderLayer.HTML, Shapes.Rectangle)
                    {
                        Title = e.Item.Name
                    };

                    Diagram.Nodes.Add(node);
                    break;

                case StateChangeActionEnum.Remove:
                    Diagram.Nodes.Remove(Diagram.Nodes.FirstOrDefault(n => n.Id == e.Item.Id));
                    break;
            }

            await RefreshElementsVisibility();
        }

        /// <summary>
        /// Provides a random point inside the canvas of 800x600 where the graph is rendered
        /// </summary>
        /// <returns></returns>
        private Point GetRandomPointWithinGraphCanvas()
        {
            return new Point(random.Next(100, 1100), random.Next(100, 1100));
        }

        public void Dispose()
        {
            if (StateManagerService != null)
            {
                StateManagerService.SelectedObjectChanged -= OnSelectedObjectChanged;

                StateManagerService.GeneralObjectsChanged -= OnGeneralObjectsChanged;
                StateManagerService.RelationshipsChanged -= OnRelationshipsChanged;

                StateManagerService.ObjectItemPropertyChanged -= OnObjectItemPropertyChanged;
                StateManagerService.RelationshipItemPropertyChanged -= OnRelationshipItemPropertyChanged;

                StateManagerService.Reload -= OnReloadRequest;
            }
        }
    }
}