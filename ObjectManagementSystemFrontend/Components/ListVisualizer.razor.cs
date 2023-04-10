using Microsoft.AspNetCore.Components;
using ObjectManagementSystemFrontend.Models;
using Radzen.Blazor;

namespace ObjectManagementSystemFrontend.Components
{
	public partial class ListVisualizer
	{


		[CascadingParameter]
		private List<GeneralObject> Objects { get; set; }

		[CascadingParameter]
		private List<Relationship> Relationships { get; set; }

		private RadzenDataGrid<GeneralObject> dataGrid;

		public void Reload()
		{
			dataGrid.Reload();
		}

		protected override void OnInitialized()
		{

		}
	}
}