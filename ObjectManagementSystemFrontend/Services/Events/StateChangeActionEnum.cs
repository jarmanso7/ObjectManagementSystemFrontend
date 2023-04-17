namespace ObjectManagementSystemFrontend.Services.Events
{
    /// <summary>
    /// Indicates if the action that triggered a StateChange event is an addition, a deletion or an update.
    /// </summary>
    public enum StateChangeActionEnum
    {
        Add,
        Remove,
        Update,
        Default
    }
}