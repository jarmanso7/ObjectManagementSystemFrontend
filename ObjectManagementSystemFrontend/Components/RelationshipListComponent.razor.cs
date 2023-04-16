﻿using Microsoft.AspNetCore.Components;
using ObjectManagementSystemFrontend.Models;
using ObjectManagementSystemFrontend.Services;
using Radzen.Blazor;
using System.Collections.ObjectModel;

namespace ObjectManagementSystemFrontend.Components
{
    /// <summary>
    /// Displays interactive information about the relationships of a particular object.
    /// </summary>
    /// <seealso cref="ComponentBase" />
    public partial class RelationshipListComponent
    {
        RadzenDataGrid<Relationship> dataGrid;

        private List<Relationship> relationships;

        private GeneralObject selectedObject;

        private Relationship relationshipToInsert;
        private Relationship relationshipToUpdate;

        private RadzenDropDown<GeneralObject> fromDropDown;
        private RadzenDropDown<GeneralObject> toDropDown;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            selectedObject = StateManagerService.SelectedObject;

            StateManagerService.SelectedObjectChanged += OnSelectedObjectChanged;
            StateManagerService.GeneralObjectsChanged += OnGeneralObjectsChanged;
            StateManagerService.ObjectItemPropertyChanged += OnObjectItemPropertyChanged;
            StateManagerService.RelationshipsChanged += OnRelationshipsChanged;
        }

        private void OnRelationshipsChanged(object src, StateChangedEventArgs<ObservableCollection<Relationship>> args)
        {
            Refresh();
        }

        private void OnObjectItemPropertyChanged(object? sender, StateChangedEventArgs<GeneralObject> e)
        {
            Refresh();
        }

        private void OnGeneralObjectsChanged(object? sender, StateChangedEventArgs<ObservableCollection<GeneralObject>> e)
        {
            Refresh();
        }

        private void OnSelectedObjectChanged(object? sender, StateChangedEventArgs<GeneralObject> e)
        {
            selectedObject = e.Item;

            Refresh();
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
            StateManagerService.InvokeRelationshipItemPropertyChanged(this, new StateChangedEventArgs<Relationship>("Relationships", relationship));

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

            if (StateManagerService.Relationships.Contains(relationship))
            {
                StateManagerService.Relationships.Remove(relationship);

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
            relationshipToInsert = new Relationship
            {
                Id = Guid.NewGuid().ToString(),
                From = new GeneralObject(),
                To = new GeneralObject()
            };

            await dataGrid.InsertRow(relationshipToInsert);
        }

        void OnCreateRow(Relationship relationship)
        {
            // TODO Call the API to ADD the general object and ADD in memory collections
            StateManagerService.Relationships.Add(relationship);

            relationshipToInsert = null;
        }

        string SectionTitle()
        {
            if (IsDisabled())
            {
                return "Relationships";
            }

            return $"{selectedObject.Name}'s relationships";
        }

        string NoRecordsText()
        {
            return "None.";
        }

        bool IsDisabled()
        {
            if (string.IsNullOrEmpty(selectedObject?.Name))
            {
                return true;
            }

            return false;
        }

        private void Refresh()
        {
            relationships = StateManagerService.Relationships.Where(r => r.From.Id == selectedObject.Id || r.To.Id == selectedObject.Id).ToList();

            Reset();

            this.StateHasChanged();
        }

        private IEnumerable<string> GetAllRelationshipTypes()
        {
            return StateManagerService.Relationships.GroupBy(r => r.Type).Select(grp => grp.First().Type);
        }

        private IEnumerable<GeneralObject> GetDropDownData(RadzenDropDown<GeneralObject> theOtherDropDown)
        {
            if(theOtherDropDown?.Value == null)
            {
                return StateManagerService.GeneralObjects.AsEnumerable();
            }

            if (theOtherDropDown.Value == selectedObject)
            {
                return StateManagerService.GeneralObjects.Where(go => go.Id != selectedObject.Id);
            }
               
            return new GeneralObject[] { selectedObject }.AsEnumerable();
        }

        private void OnDropDownChange(object args, RadzenDropDown<GeneralObject> theOtherDropDown)
        {
            var dropDownSelectedObject = args as GeneralObject;

            if (dropDownSelectedObject == selectedObject)
            {
                theOtherDropDown.Reset();
            }
        }
    }
}
