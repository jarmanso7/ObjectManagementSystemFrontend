using ObjectManagementSystemFrontend.Models;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace ObjectManagementSystemFrontend.Services
{
    /// <summary>
    ///     Manages the in-memory data and orchestrates changes accross the frontend application.
    /// </summary>
    public class StateManagerService : IDisposable
    {
        /// <summary>
        ///     Indicates whether the initial load of data has been performed or not.
        ///     StateManagerService uses this flag to determine if data is to
        ///     be saved to the backend when a relevant event is triggered (true),
        ///     or if it should no attempt to persist data to the backend because the
        ///     application is not fully initialized.
        /// </summary>

        private bool dataIsInitialized = false;

        private readonly DataProvisionService dataProviderService;
        private readonly DataPersistenceService dataPersistenceService;

        public StateManagerService(
                    DataProvisionService dataProviderService,
                    DataPersistenceService dataPersistenceService)
        {
            this.dataProviderService = dataProviderService;
            this.dataPersistenceService = dataPersistenceService;

            GeneralObjects.CollectionChanged += OnGeneralObjectsCollectionChanged;
            Relationships.CollectionChanged += OnRelationshipsCollectionChanged;
        }

        /// <summary>
        /// Keeps track in-memory of the active object selected in the search datagrid and used to show the associated relationships.
        /// </summary>
        private GeneralObject selectedObject = new GeneralObject { Description = "", Type = "", Id = "", Name = "" };
        public GeneralObject SelectedObject
        {
            get => selectedObject;
            set
            {
                if (selectedObject != value)
                {
                    selectedObject = value;
                    SelectedObjectChanged?.Invoke(this, new StateChangedEventArgs<GeneralObject>(null, value));
                }
            
            }
        }

        /// <summary>
        ///   The collection of existing relationships between objects.
        /// </summary>
        /// <value>
        /// The relationships.
        /// </value>
        public ObservableCollection<Relationship> Relationships { get; set; } = new();

        /// <summary>
        ///  The collection of general objects existing in the Object Management System.
        /// </summary>
        /// <value>
        /// The general objects.
        /// </value>
        public ObservableCollection<GeneralObject> GeneralObjects { get; set; } = new();

        /// <summary>
        /// Requests a general reload of the UI components
        /// </summary>
        public event EventHandler Reload;
        /// <summary>
        /// Notifies of a change of the <see cref="SelectedObject"/>
        /// </summary>
        public event EventHandler<GeneralObject> SelectedObjectChanged;

        /// <summary>
        /// Notifies on a change in the collection <see cref="Relationships"/>
        /// </summary>
        public event EventHandler<ObservableCollection<Relationship>> RelationshipsChanged;

        /// <summary>
        /// Notifies on a change in the collection <see cref="GeneralObjects"/>
        /// </summary>
        public event EventHandler<ObservableCollection<GeneralObject>> GeneralObjectsChanged;

        /// <summary>
        /// Notifies on a change in a property of any item in the collection <see cref="GeneralObjects"/>
        /// </summary>
        public event EventHandler<GeneralObject> ObjectItemPropertyChanged;

        /// <summary>
        /// Notifies on a change in a property of any item in the collection <see cref="Relationships"/>
        /// </summary>
        public event EventHandler<Relationship> RelationshipItemPropertyChanged;

        /// <summary>
        /// Enables invocation of ObjectItemPropertyChanged from any component that uses <see cref="StateManager"/>
        /// </summary>
        public async Task InvokeObjectItemPropertyChanged(object? sender, StateChangedEventArgs<GeneralObject> e)
        {
            ObjectItemPropertyChanged?.Invoke(this, e);

            if (dataIsInitialized)
            {
                await dataPersistenceService.UpdateGeneralObject(e.Item);
            }
        }

        /// <summary>
        /// Enables invocation of RelationshipItemPropertyChanged from any component that uses <see cref="StateManager"/>
        /// </summary>
        public async Task InvokeRelationshipItemPropertyChanged(object? sender, StateChangedEventArgs<Relationship> e)
        {
            RelationshipItemPropertyChanged?.Invoke(this, e);

            if (dataIsInitialized)
            {
                await dataPersistenceService.UpdateRelationship(e.Item);
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            GeneralObjects.CollectionChanged -= OnGeneralObjectsCollectionChanged;
            Relationships.CollectionChanged -= OnRelationshipsCollectionChanged;
        }

        /// <summary>
        /// Initialization of the root page component. All the existing Objects and their relationships are loaded at once from the backend on initialization.
        /// </summary>
        public async Task Initialize()
        {
            var initialDataLoad = await dataProviderService.Read();

            var generalObjects = initialDataLoad.Item1;
            var relationships = initialDataLoad.Item2;

            foreach (var generalObject in generalObjects)
            {
                GeneralObjects.Add(generalObject);
            }

            foreach(var relationship in relationships)
            {
                Relationships.Add(relationship);
            }

            dataIsInitialized = true;

            Reload.Invoke(this, new EventArgs());
        }

        private async void OnGeneralObjectsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            GeneralObjectsChanged?.Invoke(this, new StateChangedEventArgs<ObservableCollection<GeneralObject>>("GeneralObjects", GeneralObjects));

            if (dataIsInitialized)
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        await dataPersistenceService.CreateGeneralObject(e.NewItems?.Cast<GeneralObject>().FirstOrDefault());
                        break;

                    case NotifyCollectionChangedAction.Remove:
                        
                        //TODO: handle hanging relationships when removing an object
                        await dataPersistenceService.DeleteGeneralObject(e.OldItems?.Cast<GeneralObject>().FirstOrDefault());
                        break;
                }
            };
        }

        private async void OnRelationshipsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            RelationshipsChanged?.Invoke(this, new StateChangedEventArgs<ObservableCollection<Relationship>>("Relationships", Relationships));

            if (dataIsInitialized)
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        await dataPersistenceService.CreateRelationship(e.NewItems?.Cast<Relationship>().FirstOrDefault());
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        await dataPersistenceService.DeleteRelationship(e.OldItems?.Cast<Relationship>().FirstOrDefault());
                        break;
                }
            };
        }
    }
}