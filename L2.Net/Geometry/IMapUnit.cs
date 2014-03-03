namespace L2.Net
{
    /// <summary>
    /// Interface, that must have all objects, that have <see cref="Position"/> on the world.
    /// </summary>
    public interface IMapUnit
    {
        /// <summary>
        /// Object <see cref="Position"/>.
        /// </summary>
        Position Position { get; }
    }
}
