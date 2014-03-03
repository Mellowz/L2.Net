using System;

namespace L2.Net.Geometry
{
    /// <summary>
    /// <see cref="Sphere"/> zone implementation.
    /// </summary>
    public struct Sphere : IZone, IRounded
    {
        /// <summary>
        /// <see cref="Sphere"/> center position.
        /// </summary>
        private Vertex m_Center;
        /// <summary>
        /// <see cref="Sphere"/> radius.
        /// </summary>
        private float m_Radius;
        /// <summary>
        /// Pre-calculated powered radius value.
        /// </summary>
        private float m_PoweredRadius;

        /// <summary>
        /// Initializes new instance of <see cref="Sphere"/> struct.
        /// </summary>
        /// <param name="center"><see cref="Sphere"/> center position.</param>
        /// <param name="radius">Sphere radius.</param>
        public Sphere( Vertex center, float radius )
        {
            m_Center = center;
            m_Radius = radius;
            m_PoweredRadius = ( float )Math.Pow(radius, 2);
        }

        /// <summary>
        /// Indicates if provided <see cref="Point3D"/> is inside current <see cref="Sphere"/>.
        /// </summary>
        /// <param name="p"><see cref="Point3D"/> thats coordinates must be validated.</param>
        /// <returns>True, if provided <see cref="Point3D"/> is inside current <see cref="Sphere"/>, otherwise false.</returns>
        public bool IsInside( Point3D p )
        {
            return Math.Pow(p.X - m_Center.X, 2) + Math.Pow(p.Y - m_Center.Y, 2) + Math.Pow(p.Z - m_Center.Z, 2) <= m_PoweredRadius;
        }

        /// <summary>
        /// Gets the distance between provided <see cref="Point3D"/> and current <see cref="Sphere"/> object.
        /// </summary>
        /// <param name="p"><see cref="Point3D"/> to validate.</param>
        /// <returns>Distance between provided <see cref="Point3D"/> and current <see cref="Sphere"/> object.</returns>
        public float DistanceTo( Point3D p )
        {
            return ( float )Math.Sqrt(( Math.Pow(m_Center.X - p.X, 2d) + Math.Pow(m_Center.Y - p.Y, 2d) ) + Math.Pow(m_Center.Z - p.Z, 2d)) - m_Radius;
        }

        /// <summary>
        /// Gets or sets current <see cref="Sphere"/> center as <see cref="Vertex"/>.
        /// </summary>
        public Vertex Center
        {
            get { return m_Center; }
            set { m_Center = value; }
        }

        /// <summary>
        /// Gets or sets current <see cref="Sphere"/> object radius.
        /// </summary>
        public float Radius
        {
            get { return m_Radius; }
            set
            {
                m_Radius = value;
                m_PoweredRadius = ( float )Math.Pow(m_Radius, 2d);
            }
        }
    }
}
