using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;

namespace ObjectManagementSystemFrontend.Components
{
    public partial class GraphVisualizer
    {
        private Diagram Diagram { get; set; }

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

			Setup();
        }

		private void Setup()
		{

			var node1 = new NodeModel(new Point(50, 50), RenderLayer.HTML, Shapes.Rectangle);
			var node2 = new NodeModel(new Point(300, 300), RenderLayer.HTML, Shapes.Rectangle);
			var node3 = new NodeModel(new Point(500, 50), RenderLayer.HTML, Shapes.Rectangle);


            Diagram.Nodes.Add(new NodeModel[]{
                node1,
                node2,
                node3
            });

            Diagram.Links.Add(new LinkModel(node1, node2)
            {
                SourceMarker = LinkMarker.Arrow,
                TargetMarker = LinkMarker.Arrow
            });
		}
	}
}