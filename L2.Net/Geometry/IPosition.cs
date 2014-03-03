namespace L2.Net
{
    /// <summary>
    /// Angle-oriented <see cref="IPoint3D"/>.
    /// </summary>
    public interface IPosition : IPoint3D
    {
        /// <summary>
        /// Rotation angle.
        /// </summary>
        int Angle { get; set; }

        /// <summary>
        /// Returns distance between current <see cref="IPosition"/> and provided.
        /// </summary>
        /// <param name="other"><see cref="IPosition"/>, calculate distance to.</param>
        /// <returns>Distance between current <see cref="IPosition"/> and provided.</returns>
        float DistanceTo( IPosition other );

        /// <summary>
        /// Returns distance between current <see cref="IPosition"/> and provided <see cref="Position"/>.
        /// </summary>
        /// <param name="other"><see cref="Position"/>, calculate distance to.</param>
        /// <returns>Distance between current <see cref="IPosition"/> and provided.</returns>
        float DistanceTo( Position other );
    }
}
