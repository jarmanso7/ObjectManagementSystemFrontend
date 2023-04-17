using ObjectManagementSystemFrontend.Components;


namespace ObjectManagementSystemFrontend.Pages
{
    /// <summary>
    /// Main page of the application that contains the 3 individual components Graph, List of Objects and List of Relationships.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Components.ComponentBase" />
    public partial class Index
	{
		private GraphComponent graphComponent;

		private ObjectListComponent objectListComponent;

		private RelationshipListComponent relationshipListComponent;


        protected override void OnInitialized()
		{
			base.OnInitialized();
        }

		protected override async Task OnInitializedAsync()
		{
			await PopulateData();
		}

		private async Task PopulateData()
		{
			await StateManagerService.Initialize();
		}
	}
}