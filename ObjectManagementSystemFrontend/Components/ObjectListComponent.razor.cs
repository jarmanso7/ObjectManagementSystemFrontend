using Microsoft.AspNetCore.Components;
using ObjectManagementSystemFrontend.Models;
using ObjectManagementSystemFrontend.Services;
using Radzen.Blazor;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices.ObjectiveC;

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

            StateManager.GeneralObjectsChanged += OnGeneralObjectsChanged;
			StateManager.ObjectItemPropertyChanged += OnObjectItemPropertyChanged;
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
			StateManager.InvokeObjectItemPropertyChanged(this, new StateChangedEventArgs<GeneralObject>("GeneralObjects", generalObject));
        }

		void OnSelectRow(GeneralObject selectedObject)
		{
			StateManager.SelectedObject = selectedObject;
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

			if (StateManager.GeneralObjects.Contains(generalObject))
			{
				StateManager.GeneralObjects.Remove(generalObject);

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
			// TODO Call the API to ADD the general object and ADD in memory collections
			StateManager.GeneralObjects.Add(generalObject);

			generalObjectToInsert = null;
		}

        string NoRecordsText()
        {
            return "None. Click on \"Add New\" to create an object.";
        }
    }
}