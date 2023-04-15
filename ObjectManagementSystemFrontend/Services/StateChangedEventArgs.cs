namespace ObjectManagementSystemFrontend.Services
{
    public delegate void EventHandler<T>(object src, StateChangedEventArgs<T> args);

    public class StateChangedEventArgs<T> : EventArgs
    {
        private readonly T item;
        public T Item
        {
            get => item;
        }

        private string? collectionName;
        public string? CollectionName
        {
            get => collectionName;
        }

        public StateChangedEventArgs(string? collectionName, T item)
        {
            this.collectionName = collectionName;
            this.item = item;
        }
    }
}
