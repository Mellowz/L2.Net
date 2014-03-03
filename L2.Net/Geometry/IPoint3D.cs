using L2.Net.Geometry;

namespace L2.Net
{
    /// <summary>
    /// 3-i point interface. 
    /// </summary>
    public interface IPoint3D : IGeometrical
    {
        /// <summary>
        /// X-coordinate.
        /// </summary>
        int X { get; set; }

        /// <summary>
        /// Y-coordinate.
        /// </summary>
        int Y { get; set; }

        /// <summary>
        /// Z-coordinate.
        /// </summary>
        int Z { get; set; }

        /// <summary>
        /// Returns distance between current <see cref="IPoint3D"/> and provided.
        /// </summary>
        /// <param name="other"><see cref="IPoint3D"/> to calculate distance to.</param>
        /// <returns>Distance between current <see cref="IPoint3D"/> and some other.</returns>
        float DistanceTo( IPoint3D other );

        /// <summary>
        /// Returns distance between current <see cref="IPoint3D"/> and provided <see cref="Point3D"/>.
        /// </summary>
        /// <param name="other"><see cref="Point3D"/> to calculate distance to.</param>
        /// <returns>Distance between current <see cref="IPoint3D"/> and provided <see cref="Point3D"/>.</returns>
        float DistanceTo( Point3D other );
    }
}
