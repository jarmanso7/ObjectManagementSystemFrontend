using ObjectManagementSystemFrontend.Components;


namespace ObjectManagementSystemFrontend.Pages
{
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