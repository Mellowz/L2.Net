using System.Collections.Generic;

namespace L2.Net.Geometry
{
    /// <summary>
    /// 3-dimensional objects interface.
    /// </summary>
    public interface IGeometrical
    {
        /// <summary>
        /// Returns distance between current <see cref="IGeometrical"/> object and provided <see cref="Point3D"/>.
        /// </summary>
        /// <param name="p"><see cref="Point3D"/> object to calculate distance to.</param>
        /// <returns>Distance between current <see cref="IGeometrical"/> object and provided <see cref="Point3D"/>.</returns>
        float DistanceTo( Point3D p );
    }

    /// <summary>
    /// Base interface for rounded geometrical objects.
    /// </summary>
    public interface IRounded : IGeometrical
    {
        /// <summary>
        /// Gets or sets <see cref="IGeometrical"/> object center position.
        /// </summary>
        Vertex Center { get; set; }
        /// <summary>
        /// Gets or sets <see cref="IGeometrical"/> object radius.
        /// </summary>
        float Radius { get; set; }
    }

    /// <summary>
    /// Base zones interface.
    /// </summary>
    public interface IZone : IGeometrical
    {
        /// <summary>
        /// Indicates if provided <see cref="Point3D"/> is inside current <see cref="IZone"/> object.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        bool IsInside( Point3D p );
    }

    /// <summary>
    /// Multi-planed objects interface.
    /// </summary>
    public interface IMultiPlanedObject : IZone
    {
        /// <summary>
        /// Normalization method.
        /// </summary>
        void Normalize();

        /// <summary>
        /// Indicates if current <see cref="IMultiPlanedObject"/> was normalized yet.
        /// </summary>
        bool Normalized { get; }

        /// <summary>
        /// Triangulation method.
        /// </summary>
        void Triangulate();

        /// <summary>
        /// Indicates if current <see cref="IMultiPlanedObject"/> was triangulated yet.
        /// </summary>
        bool Triangulated { get; }
    }
}
