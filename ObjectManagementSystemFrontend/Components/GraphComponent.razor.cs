using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
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

        private Diagram Diagram { get; set; }

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

        private void OnSelectedObjectChanged(object src, StateChangedEventArgs<GeneralObject> args)
        {
            if (args?.Item?.Name == null)
            {
                return;
            }

            var selectedNode = Diagram.Nodes.FirstOrDefault(n => n.Id == args.Item.Id);

            if (selectedNode == null)
            {
                return;
            }

            selectedNode.Title = args.Item.Name;
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
        private void OnRelationshipItemPropertyChanged(object? sender, StateChangedEventArgs<Relationship> e)
        {
            
        }

        private void OnRelationshipsChanged(object? sender, StateChangedEventArgs<Relationship> e)
        {
            switch (e.Action)
            {
                case StateChangeActionEnum.Add:

                    Diagram.Links.Add(new LinkModel(Diagram.Nodes.First(n => n.Id == e.Item.From.Id), Diagram.Nodes.First(n => n.Id == e.Item.To.Id))
                    {
                        SourceMarker = LinkMarker.Arrow,
                        TargetMarker = LinkMarker.Arrow
                    });

                    break;

                case StateChangeActionEnum.Remove:

                    Diagram.Links.Remove(Diagram.Links.First(n => n.Id == e.Item.Id));
                    break;
            }
        }

        private void OnGeneralObjectsChanged(object? sender, StateChangedEventArgs<GeneralObject> e)
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