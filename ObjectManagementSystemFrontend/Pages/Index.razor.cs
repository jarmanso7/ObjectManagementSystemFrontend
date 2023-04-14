using ObjectManagementSystemFrontend.Components;
using ObjectManagementSystemFrontend.Models;
using System.Net.Http.Json;

namespace ObjectManagementSystemFrontend.Pages
{
	public partial class Index
	{
		private const string requestObjectsEndpoint = "objects";
		private const string requestRelationshipsEndpoint = "relationships";

		private List<GeneralObject> objects = new List<GeneralObject>();

		private GraphComponent graphComponent;

		private ObjectListComponent objectListComponent;

		private RelationshipListComponent relationshipListComponent;


        protected override void OnInitialized()
		{
			base.OnInitialized();
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
				StateManager.Relationships.Clear();

				foreach (var relationship in fetchedRelationships)
				{
                    StateManager.Relationships.Add(relationship);
                }
			}

			await objectListComponent.Reload();
			await relationshipListComponent.Reload();
			graphComponent.Reload();
		}

		private string getApiEndpoint(string relativePath)
		{
			return Configuration["ApiRootUri"] + relativePath;
		}
	}
}