using ObjectManagementSystemFrontend.Components;
using ObjectManagementSystemFrontend.Models;
using System.Collections.ObjectModel;
using System.Net.Http.Json;

namespace ObjectManagementSystemFrontend.Pages
{
	public partial class Index
	{
		private const string requestObjectsEndpoint = "objects";
		private const string requestRelationshipsEndpoint = "relationships";

		private GeneralObject selectedObject;

		private ObservableCollection<GeneralObject> objects = new();

		private ObservableCollection<Relationship> relationships = new();

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
				foreach (var fetchedObject in fetchedObjects)
				{
					objects.Add(fetchedObject);
				}
			}

			if (fetchedRelationships != null)
			{
                foreach (var fetchedRelationship in fetchedRelationships)
                {
                    relationships.Add(fetchedRelationship);
                }
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