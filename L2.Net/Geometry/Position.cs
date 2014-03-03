using System;

namespace L2.Net
{
    /// <summary>
    /// Represent angle-oriented 3-i point struct.
    /// </summary>
    public struct Position : IPosition
    {
        /// <summary>
        /// Zero <see cref="Position"/> value.
        /// </summary>
        public static readonly Position Zero = new Position(0, 0, 0, 0);

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
        /// Rotation angle.
        /// </summary>
        private int m_Angle;

        /// <summary>
        /// Initializes new instance of <see cref="Position"/> struct.
        /// </summary>
        /// <param name="x">X-coordinate.</param>
        /// <param name="y">Y-coordinate.</param>
        /// <param name="z">Z-coordinate.</param>
        public Position( int x, int y, int z )
        {
            m_X = x;
            m_Y = y;
            m_Z = z;
            m_Angle = 0;
        }

        /// <summary>
        /// Initializes new instance of <see cref="Position"/> struct.
        /// </summary>
        /// <param name="x">X-coordinate.</param>
        /// <param name="y">Y-coordinate.</param>
        /// <param name="z">Z-coordinate.</param>
        /// <param name="angle">Rotation angle.</param>
        public Position( int x, int y, int z, int angle )
        {
            m_X = x;
            m_Y = y;
            m_Z = z;
            m_Angle = angle;
        }

        #region IPosition Members

        /// <summary>
        /// Gets or sets <see cref="Position"/> rotation angle.
        /// </summary>
        public int Angle
        {
            get { return m_Angle; }
            set { m_Angle = value; }
        }

        /// <summary>
        /// Returns distance between current <see cref="IPosition"/> and provided.
        /// </summary>
        /// <param name="other"><see cref="IPosition"/>, calculate distance to.</param>
        /// <returns>Distance between current <see cref="IPosition"/> and provided.</returns>
        public float DistanceTo( IPosition other )
        {
            return ( float )Math.Sqrt(( m_X - other.X ) * ( m_X - other.X ) + ( m_Y - other.Y ) * ( m_Y - other.Y ) + ( m_Z - other.Z ) * ( m_Z - other.Z ));
        }

        /// <summary>
        /// Returns distance between current <see cref="IPosition"/> and provided <see cref="Position"/>.
        /// </summary>
        /// <param name="other"><see cref="Position"/>, calculate distance to.</param>
        /// <returns>Distance between current <see cref="IPosition"/> and provided.</returns>
        public float DistanceTo( Position other )
        {
            return ( float )Math.Sqrt(( m_X - other.X ) * ( m_X - other.X ) + ( m_Y - other.Y ) * ( m_Y - other.Y ) + ( m_Z - other.Z ) * ( m_Z - other.Z ));
        }

        #endregion

        #region IPoint3D Members

        /// <summary>
        /// Gets or sets <see cref="Position"/> X-coordinate.
        /// </summary>
        public int X
        {
            get { return m_X; }
            set { m_X = value; }
        }

        /// <summary>
        /// Gets or sets <see cref="Position"/> Y-coordinate.
        /// </summary>
        public int Y
        {
            get { return m_Y; }
            set { m_Y = value; }
        }

        /// <summary>
        /// Gets or sets <see cref="Position"/> Z-coordinate.
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
            return ( float )Math.Sqrt(( m_X - other.X ) * ( m_X - other.X ) + ( m_Y - other.Y ) * ( m_Y - other.Y ) + ( m_Z - other.Z ) * ( m_Z - other.Z ));
        }

        /// <summary>
        /// Returns distance between current <see cref="Point3D"/> and other <see cref="IPoint3D"/>.
        /// </summary>
        /// <param name="other"><see cref="IPoint3D"/> distance to which is needed to calculate.</param>
        /// <returns><see cref="double"/> value, that is distance between current and other <see cref="IPoint3D"/> objects.</returns>
        public float DistanceTo( IPoint3D other )
        {
            return ( float )Math.Sqrt(( m_X - other.X ) * ( m_X - other.X ) + ( m_Y - other.Y ) * ( m_Y - other.Y ) + ( m_Z - other.Z ) * ( m_Z - other.Z ));
        }


        #endregion

        /// <summary>
        /// Current <see cref="Position"/> struct <see cref="Point3D"/> representation.
        /// </summary>
        public Point3D Point3D
        {
            get { return new Point3D(m_X, m_Y, m_Z); }
            set
            {
                m_X = value.X;
                m_Y = value.Y;
                m_Z = value.Z;
            }
        }
    }
}
