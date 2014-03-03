namespace L2.Net
{
    /// <summary>
    /// Services audition types.
    /// </summary>
    public enum ServiceAuditType : byte
    {
        /// <summary>
        /// Service started event.
        /// </summary>
        ServiceStarted,
        /// <summary>
        /// Network listener started event.
        /// </summary>
        NetworkListenerStarted,
        /// <summary>
        /// Service connected event.
        /// </summary>
        Connected,
        /// <summary>
        /// Service disconnected event.
        /// </summary>
        Disconnected,
        /// <summary>
        /// Service terminated event.
        /// </summary>
        Terminated,
        /// <summary>
        /// Server goes down event.
        /// </summary>
        ShuttedDown
    }
}