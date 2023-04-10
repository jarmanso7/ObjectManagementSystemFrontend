using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Microsoft.AspNetCore.Components;
using ObjectManagementSystemFrontend.Models;

namespace ObjectManagementSystemFrontend.Components
{
	public partial class GraphVisualizer
    {
        // Used to position nodes randomly accross the canvas.
        private Random random = new Random();

        private Diagram Diagram { get; set; }

        [CascadingParameter]
		private List<GeneralObject> Objects { get; set; }

		[CascadingParameter]
		private List<Relationship> Relationships { get; set; }

		public void LoadData()
		{
			foreach (var genericObject in Objects)
			{
				var node = new NodeModel(genericObject.Id, GetRandomPointWithinGraphCanvas(), RenderLayer.HTML, Shapes.Rectangle)
				{
					Title = genericObject.Name
				};

				Diagram.Nodes.Add(node);
			}

			foreach (var relationship in Relationships)
			{
				Diagram.Links.Add(new LinkModel(Diagram.Nodes.First(n => n.Id == relationship.FromId), Diagram.Nodes.First(n => n.Id == relationship.ToId))
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
                Links = new DiagramLinkOptions
                {
                    // Options related to links
                },
                Zoom = new DiagramZoomOptions
                {
                    Minimum = 0.5, // Minimum zoom value
                    Inverse = false // mouse wheel zoom direction
                }
            };

			Diagram = new Diagram(options); 
        }

		/// <summary>
		/// Provides a random point inside the canvas of 800x600 where the graph is rendered
		/// </summary>
		/// <returns></returns>
		private Point GetRandomPointWithinGraphCanvas()
        {
            return new Point(random.Next(100, 700), random.Next(11, 500));
        }
	}
}