﻿using ObjectManagementSystemFrontend.Models;
using ObjectManagementSystemFrontend.Services;
using Radzen.Blazor;
using System.Collections.ObjectModel;

namespace ObjectManagementSystemFrontend.Components
{
    /// <summary>
    /// Displays interactive information about the available objects.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Components.ComponentBase" />
    public partial class ObjectListComponent
	{
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

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            StateManagerService.GeneralObjectsChanged += OnGeneralObjectsChanged;
			StateManagerService.ObjectItemPropertyChanged += OnObjectItemPropertyChanged;
        }

        private void OnGeneralObjectsChanged(object? sender, StateChangedEventArgs<ObservableCollection<GeneralObject>> e)
        {
			this.StateHasChanged();
        }

		private void OnObjectItemPropertyChanged(object? sender, StateChangedEventArgs<GeneralObject> e)
		{
			if (sender != this)
			{
				this.StateHasChanged();
			}
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

			//Trigger Save to DB and memory
			StateManagerService.InvokeObjectItemPropertyChanged(this, new StateChangedEventArgs<GeneralObject>("GeneralObjects", generalObject));
        }

		void OnSelectRow(GeneralObject selectedObject)
		{
			StateManagerService.SelectedObject = selectedObject;
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

			if (StateManagerService.GeneralObjects.Contains(generalObject))
			{
				StateManagerService.GeneralObjects.Remove(generalObject);

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
			generalObjectToInsert = new GeneralObject
			{
				Id = Guid.NewGuid().ToString()
			};
			
			await dataGrid.InsertRow(generalObjectToInsert);
		}

		void OnCreateRow(GeneralObject generalObject)
		{
			StateManagerService.GeneralObjects.Add(generalObject);

			generalObjectToInsert = null;
		}

        string NoRecordsText()
        {
            return "None. Click on \"Add New\" to create an object.";
        }

        private IEnumerable<string> GetAllObjectTypes()
        {
            return StateManagerService.GeneralObjects.GroupBy(r => r.Type).Select(grp => grp.First().Type);
        }
    }
}