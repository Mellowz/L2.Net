using System;

namespace L2.Net
{
    /// <summary>
    /// Represents 3-i point struct.
    /// </summary>
    public struct Point3D : IPoint3D
    {
        /// <summary>
        /// Zero <see cref="Point3D"/> value.
        /// </summary>
        public static readonly Point3D Zero = new Point3D(0, 0, 0);

        /// <summary>
        /// X-coordinate.
        /// </summary>
        private int m_X;

        /// <summary>
        /// Y-coordinate.
        /// </summary>
        private int m_Y;

        /// <summary>
        /// Z-coordinate.
        /// </summary>
        private int m_Z;

        /// <summary>
        /// Initializes new instance of <see cref="Point3D"/> struct.
        /// </summary>
        /// <param name="x">X-coordinate.</param>
        /// <param name="y">Y-coordinate.</param>
        /// <param name="z">Z-coordinate.</param>
        public Point3D( int x, int y, int z )
        {
            m_X = x;
            m_Y = y;
            m_Z = z;
        }

        #region IPoint3D Members

        /// <summary>
        /// Gets or sets <see cref="Point3D"/> X-coordinate.
        /// </summary>
        public int X
        {
            get { return m_X; }
            set { m_X = value; }
        }

        /// <summary>
        /// Gets or sets <see cref="Point3D"/> Y-coordinate.
        /// </summary>
        public int Y
        {
            get { return m_Y; }
            set { m_Y = value; }
        }

        /// <summary>
        /// Gets or sets <see cref="Point3D"/> Z-coordinate.
        /// </summary>
        public int Z
        {
            get { return m_Z; }
            set { m_Z = value; }
        }

        /// <summary>
        /// Returns distance between current <see cref="Point3D"/> and some other.
        /// </summary>
        /// <param name="other"><see cref="Point3D"/> distance to which is needed to calculate.</param>
        /// <returns><see cref="double"/> value, that is distance between current and some other <see cref="Point3D"/> objects.</returns>
        public float DistanceTo( Point3D other )
        {
            return (float)Math.Sqrt(( m_X - other.X ) * ( m_X - other.X ) + ( m_Y - other.Y ) * ( m_Y - other.Y ) + ( m_Z - other.Z ) * ( m_Z - other.Z ));
        }

        /// <summary>
        /// Returns distance between current <see cref="Point3D"/> and other <see cref="IPoint3D"/>.
        /// </summary>
        /// <param name="other"><see cref="IPoint3D"/> distance to which is needed to calculate.</param>
        /// <returns><see cref="double"/> value, that is distance between current and other <see cref="IPoint3D"/> objects.</returns>
        public float DistanceTo( IPoint3D other )
        {
            return (float)Math.Sqrt(( m_X - other.X ) * ( m_X - other.X ) + ( m_Y - other.Y ) * ( m_Y - other.Y ) + ( m_Z - other.Z ) * ( m_Z - other.Z ));
        }

        /// <summary>
        /// Calculates distance between two <see cref="Point3D" /> objects.
        /// </summary>
        /// <param name="a">First <see cref="Point3D"/>.</param>
        /// <param name="b">Second <see cref="Point3D"/>.</param>
        /// <returns>Distance between two <see cref="Point3D" /> objects.</returns>
        public static double DistanceBetween( Point3D a, Point3D b )
        {
            return Utility.DistanceBetween(a, b);
        }

        #endregion

        /// <summary>
        /// Current <see cref="Point3D"/> struct <see cref="Position"/> representation.
        /// </summary>
        public Position Position
        {
            get { return new Position(m_X, m_Y, m_Z); }
            set
            {
                m_X = value.X;
                m_Y = value.Y;
                m_Z = value.Z;
            }
        }
    }
}
