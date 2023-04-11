﻿using Microsoft.AspNetCore.Components;
using ObjectManagementSystemFrontend.Models;
using Radzen.Blazor;

namespace ObjectManagementSystemFrontend.Components
{
	public partial class ObjectListComponent
	{
		[CascadingParameter]
		private List<GeneralObject> Objects { get; set; }

		private RadzenDataGrid<GeneralObject> dataGrid;

		private GeneralObject generalObjectToInsert;
		private GeneralObject generalObjectToUpdate;

		public async Task Reload()
		{
			await dataGrid.Reload();
		}

		void Reset()
		{
			generalObjectToInsert = null;
			generalObjectToUpdate = null;
		}

		async Task EditRow(GeneralObject generalObject)
		{
			generalObjectToUpdate = generalObject;
			await dataGrid.EditRow(generalObject);
		}

		void OnUpdateRow(GeneralObject generalObject)
		{
			if (generalObject == generalObjectToInsert)
			{
				generalObjectToInsert = null;
			}

			generalObjectToUpdate = null;

			// TODO Call the API to update the general object and UPDATE in memory collections
		}

		async Task SaveRow(GeneralObject generalObject)
		{
			await dataGrid.UpdateRow(generalObject);
		}

		void CancelEdit(GeneralObject generalObject)
		{
			if (generalObject == generalObjectToInsert)
			{
				generalObjectToInsert = null;
			}

			generalObjectToUpdate = null;

			dataGrid.CancelEditRow(generalObject);
		}

		async Task DeleteRow(GeneralObject generalObject)
		{
			if (generalObject == generalObjectToInsert)
			{
				generalObjectToInsert = null;
			}

			if (generalObject == generalObjectToUpdate)
			{
				generalObjectToUpdate = null;
			}

			if (Objects.Contains(generalObject))
			{

				// TODO Trigger delete call to the Backend

				await dataGrid.Reload();
			}
			else
			{
				dataGrid.CancelEditRow(generalObject);
				await dataGrid.Reload();
			}
		}

		async Task InsertRow()
		{
			generalObjectToInsert = new GeneralObject();
			await dataGrid.InsertRow(generalObjectToInsert);
		}

		void OnCreateRow(GeneralObject generalObject)
		{
			// TODO Call the API to ADD the general object and ADD in memory collections

			generalObjectToInsert = null;
		}
	}
}