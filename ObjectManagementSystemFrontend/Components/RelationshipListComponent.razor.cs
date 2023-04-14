﻿using Microsoft.AspNetCore.Components;
using ObjectManagementSystemFrontend.Models;
using ObjectManagementSystemFrontend.Services;
using Radzen.Blazor;

namespace ObjectManagementSystemFrontend.Components
{
    /// <summary>
    /// Displays interactive information about the relationships of a particular object.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Components.ComponentBase" />
    public partial class RelationshipListComponent
    {
        RadzenDataGrid<Relationship> dataGrid;

        private List<Relationship> relationships;

        private GeneralObject selectedObject;

        private Relationship relationshipToInsert;
        private Relationship relationshipToUpdate;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            selectedObject = StateManager.SelectedObject;

            StateManager.SelectedObjectChanged += OnSelectedObjectChanged;
        }

        private void OnSelectedObjectChanged(object? sender, GeneralObject e)
        {
            selectedObject = e;

            relationships = StateManager.Relationships.Where(r => r.FromId == selectedObject.Id || r.ToId == selectedObject.Id).ToList();

            Reset();

            this.StateHasChanged();
        }

        public async Task Reload()
        {
            await dataGrid.Reload();
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

            if (relationships.Contains(relationship))
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