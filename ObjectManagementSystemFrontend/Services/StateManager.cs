using ObjectManagementSystemFrontend.Models;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace ObjectManagementSystemFrontend.Services
{
    /// <summary>
    /// Manages the in memory data to orchestrate changes in visualization accross the frontend app
    /// </summary>
    public class StateManager
    {
        private GeneralObject selectedObject = new GeneralObject { Description = "", Type = "", Id = "", Name = "" };
        public GeneralObject SelectedObject
        {
            get => selectedObject;
            set
            {
                if (selectedObject != value)
                {
                    selectedObject = value;
                    OnSelectedObjectChanged(value);
                }
            }
        }

        public event EventHandler<GeneralObject> SelectedObjectChanged;

        private void OnSelectedObjectChanged(GeneralObject generalObject)
        {
            SelectedObjectChanged?.Invoke(this, selectedObject);
        }

        // TODO: load objects and relationships from backend to StateManager

        private ObservableCollection<Relationship> relationships = new();
        public ObservableCollection<Relationship> Relationships
        {
            get => relationships;
            set
            {
                if (relationships != value)
                {
                    relationships = value;
                    RelationshipsChanged?.Invoke(this, relationships);
                }
            }
        }

        public event EventHandler<ObservableCollection<Relationship>> RelationshipsChanged;

        private ObservableCollection<GeneralObject> generalObjects = new();
        public ObservableCollection<GeneralObject> GeneralObjects
        {
            get => generalObjects;
            set
            {
                if (GeneralObjects != value)
                {
                    generalObjects = value;
                    GeneralObjectsChanged?.Invoke(this, generalObjects);
                }
            }
        }

        public event EventHandler<ObservableCollection<GeneralObject>> GeneralObjectsChanged;

        public StateManager()
        {
            generalObjects.CollectionChanged += OnGeneralObjectsCollectionChanged;
        }

        private void OnGeneralObjectsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            GeneralObjectsChanged?.Invoke(this, generalObjects);
        }
    }
}