using Microsoft.AspNetCore.Components;
using ObjectManagementSystemFrontend.Models;
using ObjectManagementSystemFrontend.Services;
using ObjectManagementSystemFrontend.Services.Events;
using Radzen.Blazor;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace ObjectManagementSystemFrontend.Components
{
    /// <summary>
    /// Displays interactive information about the relationships of a particular object.
    /// </summary>
    /// <seealso cref="ComponentBase" />
    public partial class RelationshipListComponent : IDisposable
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
            StateManagerService.Reload += OnReloadRequest;
        }

        private async void OnReloadRequest(object? sender, EventArgs e)
        {
            await dataGrid.Reload();
        }

        private void OnRelationshipsChanged(object src, StateChangedEventArgs<Relationship> args)
        {
            Refresh();
        }

        private void OnObjectItemPropertyChanged(object? sender, StateChangedEventArgs<GeneralObject> e)
        {
            Refresh();
        }

        private void OnGeneralObjectsChanged(object? sender, StateChangedEventArgs<GeneralObject> e)
        {
            Refresh();
        }

        private void OnSelectedObjectChanged(object? sender, StateChangedEventArgs<GeneralObject> e)
        {
            selectedObject = e.Item;

            Refresh();
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

        async Task OnUpdateRow(Relationship relationship)
        {
            if (relationship == relationshipToInsert)
            {
                relationshipToInsert = null;
            }

            relationshipToUpdate = null;

            await StateManagerService.InvokeRelationshipItemPropertyChanged(
                this,
                new StateChangedEventArgs<Relationship>(relationship, StateChangeActionEnum.Update));
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
            if (selectedObject != null)
            {
                relationships = StateManagerService.Relationships.Where(r => r.From.Id == selectedObject.Id || r.To.Id == selectedObject.Id).ToList();
            }
            else
            {
                relationships.Clear();
            }

            Reset();
            dataGrid.Reload();

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

        public void Dispose()
        {
            if (StateManagerService != null)
            {
                StateManagerService.SelectedObjectChanged -= OnSelectedObjectChanged;
                StateManagerService.GeneralObjectsChanged -= OnGeneralObjectsChanged;
                StateManagerService.ObjectItemPropertyChanged -= OnObjectItemPropertyChanged;
                StateManagerService.RelationshipsChanged -= OnRelationshipsChanged;
                StateManagerService.Reload -= OnReloadRequest;
            }
        }
    }
}
