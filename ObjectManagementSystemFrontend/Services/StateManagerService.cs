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
        // Indicates whether the application data has been initialized or not.
        private bool isInitialized = false;

        private readonly DataProviderService dataProviderService;
        private readonly DataPersistenceService dataPersistenceService;

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
            ObjectItemPropertyChanged?.Invoke(this, e);

            if (isInitialized)
            {
                dataPersistenceService.Update(e.Item);
            }
        }

        /// <summary>
        /// Enables invocation of RelationshipItemPropertyChanged from any component that uses <see cref="StateManager"/>
        /// </summary>
        public void InvokeRelationshipItemPropertyChanged(object? sender, StateChangedEventArgs<Relationship> e)
        {
            RelationshipItemPropertyChanged?.Invoke(this, e);

            if (isInitialized)
            {
                dataPersistenceService.Update(e.Item);
            }
        }

        public StateManagerService(DataProviderService dataProviderService,
                                    DataPersistenceService dataPersistenceService)
        {
            this.dataProviderService = dataProviderService;
            this.dataPersistenceService = dataPersistenceService;

            generalObjects.CollectionChanged += OnGeneralObjectsCollectionChanged;
            relationships.CollectionChanged += OnRelationshipsCollectionChanged;
        }

        public async Task Initialize()
        {
            var initialDataLoad = await dataProviderService.FetchData();

            foreach(var generalObject in initialDataLoad.Item1)
            {
                generalObjects.Add(generalObject);
            }

            foreach(var relationship in initialDataLoad.Item2)
            {
                relationships.Add(relationship);
            }

            isInitialized = true;
        }

        private void OnGeneralObjectsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            GeneralObjectsChanged?.Invoke(this, new StateChangedEventArgs<ObservableCollection<GeneralObject>>("GeneralObjects", generalObjects));

            if (isInitialized)
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        dataPersistenceService.Create(e.NewItems?.Cast<GeneralObject>().FirstOrDefault());
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        dataPersistenceService.Delete(e.OldItems?.Cast<GeneralObject>().FirstOrDefault());
                        break;
                }
            };
        }

        private void OnRelationshipsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            RelationshipsChanged?.Invoke(this, new StateChangedEventArgs<ObservableCollection<Relationship>>("Relationships", relationships));

            if (isInitialized)
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        dataPersistenceService.Create(e.NewItems?.Cast<Relationship>().FirstOrDefault());
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        dataPersistenceService.Delete(e.OldItems?.Cast<Relationship>().FirstOrDefault());
                        break;
                }
            };
        }

        public void Dispose()
        {
            generalObjects.CollectionChanged -= OnGeneralObjectsCollectionChanged;
            relationships.CollectionChanged -= OnRelationshipsCollectionChanged;
        }
    }
}