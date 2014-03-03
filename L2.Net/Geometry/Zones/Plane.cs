using System;

namespace L2.Net.Geometry
{
    /// <summary>
    /// Represents <see cref="Plane"/> struct.
    /// </summary>
    public struct Plane
    {
        /// <summary>
        /// Geometrical A value.
        /// </summary>
        public float A;
        /// <summary>
        /// Geometrical B value.
        /// </summary>
        public float B;
        /// <summary>
        /// Geometrical C value.
        /// </summary>
        public float C;
        /// <summary>
        /// Geometrical D value.
        /// </summary>
        public float D;
        
        /// <summary>
        /// Calculations constant.
        /// </summary>
        private readonly float fSqrt;

        /// <summary>
        /// Initializes new instance of <see cref="Plane"/> struct.
        /// </summary>
        /// <param name="vertexes">Array of <see cref="Vertex"/> objects to initialize from.</param>
        public Plane( params Vertex[] vertexes )
        {
            if ( vertexes.Length != 3 )
                throw new ArgumentOutOfRangeException("points", "L2.Net.Geometry.Plane must be initialized with 3 Vertexes.");

            Vertex v1 = vertexes[0];
            Vertex v2 = vertexes[1];
            Vertex v3 = vertexes[2];

            A = v1.Y * ( float )( v2.Z - v3.Z ) + v2.Y * ( float )( v3.Z - v1.Z ) + v3.Y * ( float )( v1.Z - v2.Z );
            B = v1.Z * ( float )( v2.X - v3.X ) + v2.Z * ( float )( v3.X - v1.X ) + v3.Z * ( float )( v1.X - v2.X );
            C = v1.X * ( float )( v2.Y - v3.Y ) + v2.X * ( float )( v3.Y - v1.Y ) + v3.X * ( float )( v1.Y - v2.Y );
            D = ( -1f ) * ( v1.X * ( float )( v2.Y * v3.Z - v3.Y * v2.Z ) + v2.X * ( float )( v3.Y * v1.Z - v1.Y * v3.Z ) + v3.X * ( float )( v1.Y * v2.Z - v2.Y * v1.Z ) );

            fSqrt = ( float )Math.Sqrt(Math.Pow(A, 2) + Math.Pow(B, 2) + Math.Pow(C, 2));
        }

        /// <summary>
        /// Calculates relative distance from current <see cref="Plane"/> object to provided <see cref="Point3D"/> object.
        /// </summary>
        /// <param name="p"><see cref="Point3D"/> to calculate distance to.</param>
        /// <returns>Relative distance from current <see cref="Plane"/> object to provided <see cref="Point3D"/> object.</returns>
        public float DistanceTo( Point3D p )
        {
            return ( A * p.X + B * p.Y + C * p.Z + D ) / fSqrt;
        }

        /// <summary>
        /// Gets absolute value of distance between provided <see cref="Point3D"/> and current <see cref="Plane"/> object.
        /// </summary>
        /// <param name="p"><see cref="Point3D"/> to calculate distance to.</param>
        /// <returns>Absolute value of distance between current <see cref="Plane"/> object and provided <see cref="Point3D"/>.</returns>
        public float AbsoluteDistanceToPoint( Point3D p )
        {
            return Utility.AbsoluteValue(DistanceTo(p));
        }
    }
}
