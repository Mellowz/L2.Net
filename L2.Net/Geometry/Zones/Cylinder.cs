using System;

namespace L2.Net.Geometry
{
    /// <summary>
    /// Represents cylindric object struct.
    /// </summary>
    public struct Cylinder : IZone, IRounded
    {
        /// <summary>
        /// <see cref="Cylinder"/> center position.
        /// </summary>
        private Vertex m_Center;
        /// <summary>
        /// <see cref="Cylinder"/> radius.
        /// </summary>
        private float m_Radius;
        /// <summary>
        /// <see cref="Cylinder"/> height.
        /// </summary>
        public readonly float Height;
        /// <summary>
        /// Pre-calculated powered radius value.
        /// </summary>
        private float m_PoweredRadius;

        /// <summary>
        /// Initializes new instance of <see cref="Cylinder"/> struct.
        /// </summary>
        /// <param name="center"><see cref="Cylinder"/> center position.</param>
        /// <param name="radius"><see cref="Cylinder"/> radius.</param>
        /// <param name="heigth"><see cref="Cylinder"/> height.</param>
        public Cylinder( Vertex center, float radius, float heigth )
        {
            m_Center = center;
            m_Radius = radius;
            Height = heigth;
            m_PoweredRadius = ( float )Math.Pow(radius, 2d);
        }

        /// <summary>
        /// Indicates if provided <see cref="Point3D"/> is inside current <see cref="Cylinder"/>.
        /// </summary>
        /// <param name="p"><see cref="Point3D"/> thats coordinates must be validated.</param>
        /// <returns>True, if provided <see cref="Point3D"/> is inside current <see cref="Cylinder"/>, otherwise false.</returns>
        public bool IsInside( Point3D p )
        {
            return Math.Pow(p.X - m_Center.X, 2) + Math.Pow(p.Y - m_Center.Y, 2) + Math.Pow(p.Z - m_Center.Z, 2) <= m_PoweredRadius;
        }

        /// <summary>
        /// Gets the distance between provided <see cref="Point3D"/> and current <see cref="Cylinder"/> object.
        /// </summary>
        /// <param name="p"><see cref="Point3D"/> to validate.</param>
        /// <returns>Distance between provided <see cref="Point3D"/> and current <see cref="Cylinder"/> object center.</returns>
        public float DistanceTo( Point3D p )
        {
            return ( float )Math.Sqrt(( Math.Pow(m_Center.X - p.X, 2d) + Math.Pow(m_Center.Y - p.Y, 2d) ) + Math.Pow(m_Center.Z - p.Z, 2d));
        }

        /// <summary>
        /// Gets or sets current <see cref="Cylinder"/> center as <see cref="Vertex"/>.
        /// </summary>
        public Vertex Center
        {
            get { return m_Center; }
            set { m_Center = value; }
        }

        /// <summary>
        /// Gets or sets current <see cref="Cylinder"/> object radius.
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
