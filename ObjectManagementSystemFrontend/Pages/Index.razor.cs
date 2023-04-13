using ObjectManagementSystemFrontend.Components;
using ObjectManagementSystemFrontend.Models;
using System.Net.Http.Json;

namespace ObjectManagementSystemFrontend.Pages
{
	public partial class Index
	{
		private const string requestObjectsEndpoint = "objects";
		private const string requestRelationshipsEndpoint = "relationships";

		private GeneralObject selectedObject;

		private List<GeneralObject> objects = new List<GeneralObject>();

		private List<Relationship> relationships = new List<Relationship>();

		private GraphComponent graphComponent;

		private ObjectListComponent objectListComponent;

		private SelectedObjectComponent selectedObjectComponent;


        protected override void OnInitialized()
		{
			base.OnInitialized();

            selectedObject = new GeneralObject
            {
                Name = "default",
                Type = "default",
                Description = "default",
                Id = "default"
            };

        }

		protected override async Task OnInitializedAsync()
		{
			await PopulateDataFromBackend();
		}

		private async Task PopulateDataFromBackend()
		{
			var fetchedObjects = await Http.GetFromJsonAsync<GeneralObject[]>(getApiEndpoint(requestObjectsEndpoint));
			var fetchedRelationships = await Http.GetFromJsonAsync<Relationship[]>(getApiEndpoint(requestRelationshipsEndpoint));

			if (fetchedObjects != null)
			{
				objects.AddRange(fetchedObjects);
			}

			if (fetchedRelationships != null)
			{
				relationships.AddRange(fetchedRelationships);
			}

			graphComponent.LoadData();
			await objectListComponent.Reload();
			await selectedObjectComponent.Reload();
		}

		private string getApiEndpoint(string relativePath)
		{
			return Configuration["ApiRootUri"] + relativePath;
		}
	}
}