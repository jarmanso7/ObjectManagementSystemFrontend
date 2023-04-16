using ObjectManagementSystemFrontend.Models;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace ObjectManagementSystemFrontend.Services
{
    /// <summary>
    /// Manages the in memory data and orchestrates changes accross the frontend application
    /// </summary>
    public class StateManagerService : IDisposable
    {
        /// <summary>
        /// Indicates if initial load of data has been performed.
        /// </summary>
        private bool dataIsLoadedFromBackend = false;

        private readonly DataProviderService backendDataSerializerService;

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

        /// <summary>
        /// Notifies of a change of the <see cref="SelectedObject"/>
        /// </summary>
        public event EventHandler<GeneralObject> SelectedObjectChanged;

        private void OnSelectedObjectChanged(GeneralObject generalObject)
        {
            SelectedObjectChanged?.Invoke(this, new StateChangedEventArgs<GeneralObject>(null, generalObject));
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
                    RelationshipsChanged?.Invoke(this, new StateChangedEventArgs<ObservableCollection<Relationship>>("Relationships", relationships));
                }
            }
        }

        /// <summary>
        /// Notifies on a change in the collection <see cref="Relationships"/>
        /// </summary>
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
                    GeneralObjectsChanged?.Invoke(this, new StateChangedEventArgs<ObservableCollection<GeneralObject>>("GeneralObjects", generalObjects));
                }
            }
        }

        /// <summary>
        /// Notifies on a change in the collection <see cref="GeneralObjects"/>
        /// </summary>
        public event EventHandler<ObservableCollection<GeneralObject>> GeneralObjectsChanged;

        /// <summary>
        /// Notifies on a change in a property of any item in the collection <see cref="GeneralObjects"/>
        /// </summary>
        public event EventHandler<GeneralObject> ObjectItemPropertyChanged;


        /// <summary>
        /// Notifies on a change in a property of any item in the collection <see cref="GeneralObjects"/> or <see cref="Relationships"/>
        /// </summary>
        public event EventHandler<Relationship> RelationshipItemPropertyChanged;

        /// <summary>
        /// Enables invocation of ObjectItemPropertyChanged from any component that uses <see cref="StateManager"/>
        /// </summary>
        public void InvokeObjectItemPropertyChanged(object? sender, StateChangedEventArgs<GeneralObject> e)
        {
            //TODO: trigger database changes using e.

            ObjectItemPropertyChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Enables invocation of RelationshipItemPropertyChanged from any component that uses <see cref="StateManager"/>
        /// </summary>
        public void InvokeRelationshipItemPropertyChanged(object? sender, StateChangedEventArgs<Relationship> e)
        {
            //TODO: trigger database changes using e.

            RelationshipItemPropertyChanged?.Invoke(this, e);
        }

        public StateManagerService(DataProviderService backendDataSerializerService)
        {
            this.backendDataSerializerService = backendDataSerializerService;

            generalObjects.CollectionChanged += OnGeneralObjectsCollectionChanged;
            relationships.CollectionChanged += OnRelationshipsCollectionChanged;
        }

        public async Task Initialize()
        {
            var initialDataLoad = await backendDataSerializerService.FetchData();

            foreach(var generalObject in initialDataLoad.Item1)
            {
                generalObjects.Add(generalObject);
            }

            foreach(var relationship in initialDataLoad.Item2)
            {
                relationships.Add(relationship);
            }

            dataIsLoadedFromBackend = true;
        }

        private void OnGeneralObjectsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            //TODO: trigger database changes using e.OldItems, e.NewItems and e.Action
            GeneralObjectsChanged?.Invoke(this, new StateChangedEventArgs<ObservableCollection<GeneralObject>>("GeneralObjects", generalObjects));
        }

        private void OnRelationshipsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            //TODO: trigger database changes using e.OldItems, e.NewItems and e.Action

            RelationshipsChanged?.Invoke(this, new StateChangedEventArgs<ObservableCollection<Relationship>>("Relationships", relationships));
        }

        public void Dispose()
        {
            generalObjects.CollectionChanged -= OnGeneralObjectsCollectionChanged;
            relationships.CollectionChanged -= OnRelationshipsCollectionChanged;
        }
    }
}