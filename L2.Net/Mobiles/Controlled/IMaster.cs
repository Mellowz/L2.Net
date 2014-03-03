namespace L2.Net.Mobiles
{
    /// <summary>
    /// Master interface.
    /// </summary>
    public interface IMaster : IMasterOrControlled
    {
        /// <summary>
        /// Gets creature, currently controlled by <see cref="IMaster"/>.
        /// </summary>
        ControlledMobile ControlledMobile { get; }
    }
}
