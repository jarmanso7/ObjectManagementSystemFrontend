using Microsoft.AspNetCore.Components;
using ObjectManagementSystemFrontend.Models;
using Radzen.Blazor;
using System.Collections.ObjectModel;

namespace ObjectManagementSystemFrontend.Components
{
    public partial class SelectedObjectComponent
    {
        RadzenDataGrid<Relationship> dataGrid;

        [CascadingParameter]
        private ObservableCollection<Relationship> Relationships { get; set; }

        [Parameter]
        public GeneralObject SelectedObject { get; set; } = new GeneralObject
        {
            Type = "default2",
            Name = "default2",
            Description = "default2",
            Id = "default2"
        };

        [Parameter]
        public EventCallback<GeneralObject> SelectedObjectChanged { get; set; }

        private Relationship relationshipToInsert;
        private Relationship relationshipToUpdate;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }


        public async Task Reload()
        {
            await dataGrid.Reload();
        }

        async Task OnSelectedObjectChanged()
        {
            await SelectedObjectChanged.InvokeAsync(SelectedObject);
        }

        void Reset()
        {
            relationshipToInsert = null;
            relationshipToUpdate = null;
        }

        async Task EditRow(Relationship relationship)
        {
            relationshipToUpdate = relationship;
            await dataGrid.EditRow(relationship);
        }

        void OnUpdateRow(Relationship relationship)
        {
            if (relationship == relationshipToInsert)
            {
                relationshipToInsert = null;
            }

            relationshipToUpdate = null;

            // TODO Call the API to update the relationship and UPDATE in memory collections
        }

        async Task SaveRow(Relationship relationship)
        {
            await dataGrid.UpdateRow(relationship);
        }

        void CancelEdit(Relationship relationship)
        {
            if (relationship == relationshipToInsert)
            {
                relationshipToInsert = null;
            }

            relationshipToUpdate = null;

            dataGrid.CancelEditRow(relationship);
        }

        async Task DeleteRow(Relationship relationship)
        {
            if (relationship == relationshipToInsert)
            {
                relationshipToInsert = null;
            }

            if (relationship == relationshipToUpdate)
            {
                relationshipToUpdate = null;
            }

            if (Relationships.Contains(relationship))
            {
                // TODO Trigger delete call to the Backend

                await dataGrid.Reload();
            }
            else
            {
                dataGrid.CancelEditRow(relationship);
                await dataGrid.Reload();
            }
        }

        async Task InsertRow()
        {
            relationshipToInsert = new Relationship();
            await dataGrid.InsertRow(relationshipToInsert);
        }

        void OnCreateRow(Relationship relationship)
        {
            // TODO Call the API to ADD the general object and ADD in memory collections

            relationshipToInsert = null;
        }
    }
}
