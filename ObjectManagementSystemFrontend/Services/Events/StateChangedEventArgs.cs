using ObjectManagementSystemFrontend.Services.Events;

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

        public StateChangeActionEnum Action { get; private set; }

        public StateChangedEventArgs(T item, StateChangeActionEnum action)
        {
            this.item = item;
            this.Action = action;
        }
    }
}