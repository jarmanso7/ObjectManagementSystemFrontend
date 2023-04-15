using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Microsoft.AspNetCore.Components;
using ObjectManagementSystemFrontend.Models;
using ObjectManagementSystemFrontend.Services;
using System.Collections.ObjectModel;

namespace ObjectManagementSystemFrontend.Components
{
	public partial class GraphComponent
    {
        // Used to position nodes randomly accross the canvas.
        private Random random = new Random();

        private Diagram Diagram { get; set; }

		private List<GeneralObject> objects;

		private List<Relationship> relationships;

		protected override async Task OnInitializedAsync()
		{
			await base.OnInitializedAsync();

            StateManager.GeneralObjectsChanged += OnGeneralObjectsChanged;
            StateManager.RelationshipsChanged += OnRelationshipsChanged;

            StateManager.ObjectItemPropertyChanged += OnObjectItemPropertyChanged;
            StateManager.RelationshipItemPropertyChanged += OnRelationshipItemPropertyChanged;
        }

        private void OnObjectItemPropertyChanged(object? sender, StateChangedEventArgs<GeneralObject> e)
        {
            Console.WriteLine($"GraphComponent: OnObjectItemPropertyChanged, object Name: {e.Item.Name}");
        }
        private void OnRelationshipItemPropertyChanged(object? sender, StateChangedEventArgs<Relationship> e)
        {
            Console.WriteLine($"GraphComponent: OnRelationshipItemPropertyChanged, relationship Type: {e.Item.Type}");
        }

        private void OnRelationshipsChanged(object? sender, StateChangedEventArgs<ObservableCollection<Relationship>> e)
        {
            Console.WriteLine("GraphComponent: OnRelationshipsChanged");
        }

        private void OnGeneralObjectsChanged(object? sender, StateChangedEventArgs<ObservableCollection<GeneralObject>> e)
        {
			Console.WriteLine("GraphComponent: OnGeneralObjectsChanged");
        }

        public void Reload()
        {
			LoadData();
        }

        public void LoadData()
		{
			relationships = StateManager.Relationships.ToList();

			objects = StateManager.GeneralObjects.ToList();

			foreach (var genericObject in objects)
			{
				var node = new NodeModel(genericObject.Id, GetRandomPointWithinGraphCanvas(), RenderLayer.HTML, Shapes.Rectangle)
				{
					Title = genericObject.Name
				};

				Diagram.Nodes.Add(node);
			}

			foreach (var relationship in relationships)
			{
				Diagram.Links.Add(new LinkModel(Diagram.Nodes.First(n => n.Id == relationship.From.Id), Diagram.Nodes.First(n => n.Id == relationship.To.Id))
				{
					SourceMarker = LinkMarker.Arrow,
					TargetMarker = LinkMarker.Arrow
				});
			}
		}

		protected override void OnInitialized()
        {
            base.OnInitialized();

            var options = new DiagramOptions
            {
                DeleteKey = "Delete",
                DefaultNodeComponent = null,
                AllowMultiSelection = true,
				AllowPanning = false,
                Links = new DiagramLinkOptions
                {
                    // Options related to links
                },
                Zoom = new DiagramZoomOptions
                {
					Enabled = false,
                }
            };

			Diagram = new Diagram(options);

			Diagram.SetZoom(0.5);
        }

		/// <summary>
		/// Provides a random point inside the canvas of 800x600 where the graph is rendered
		/// </summary>
		/// <returns></returns>
		private Point GetRandomPointWithinGraphCanvas()
        {
            return new Point(random.Next(100, 1100), random.Next(100, 1100));
        }
	}
}