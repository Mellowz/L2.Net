using System;

namespace L2.Net.Geometry
{
    /// <summary>
    /// Vertex struct.
    /// </summary>
    public struct Vertex : IGeometrical
    {
        /// <summary>
        /// In L2 math this <see cref="Vertex"/> is unaccessible.
        /// </summary>
        public static readonly Vertex Invalid = new Vertex(Int32.MinValue, Int32.MinValue, Int32.MinValue);

        /// <summary>
        /// <see cref="Vertex"/> X coordinate.
        /// </summary>
        public readonly int X;
        /// <summary>
        /// <see cref="Vertex"/> Y coordinate.
        /// </summary>
        public readonly int Y;
        /// <summary>
        /// <see cref="Vertex"/> Z coordinate.
        /// </summary>
        public readonly int Z;

        /// <summary>
        /// Initializes new instance of <see cref="Vertex"/> struct.
        /// </summary>
        /// <param name="x"><see cref="Vertex"/> X coordinate.</param>
        /// <param name="y"><see cref="Vertex"/> Y coordinate.</param>
        /// <param name="z"><see cref="Vertex"/> Z coordinate.</param>
        public Vertex( int x, int y, int z )
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Initializes new instance of <see cref="Vertex"/> struct.
        /// </summary>
        /// <param name="p"><see cref="Point3D"/> object to initialize from.</param>
        public Vertex( Point3D p )
            : this(p.X, p.Y, p.Z)
        {
        }

        /// <summary>
        /// Gets the distance between provided <see cref="Point3D"/> and current <see cref="Cylinder"/> object.
        /// </summary>
        /// <param name="p"><see cref="Point3D"/> to validate.</param>
        /// <returns>Distance between provided <see cref="Point3D"/> and current <see cref="Cylinder"/> object center.</returns>
        public float DistanceTo( Point3D p )
        {
            return ( float )Math.Sqrt(( Math.Pow(X - p.X, 2d) + Math.Pow(Y - p.Y, 2d) ) + Math.Pow(Z - p.Z, 2d));
        }

        /// <summary>
        /// Returns <see cref="string"/> representation of current <see cref="Vertex"/> struct.
        /// </summary>
        /// <returns><see cref="string"/> representation of current <see cref="Vertex"/> struct.</returns>
        public override string ToString()
        {
            return String.Format("X={0}, Y={1}, Z={2}", X, Y, Z);
        }

        /// <summary>
        /// Determines two <see cref="Vertex"/> structs equality.
        /// </summary>
        /// <param name="a">First <see cref="Vertex"/> to compare.</param>
        /// <param name="m">Second <see cref="Vertex"/> to compare.</param>
        /// <returns>True, if two provided <see cref="Vertex"/> structs are equal, otherwise false.</returns>
        public static bool operator ==( Vertex a, Vertex b )
        {
            return a.X == b.X && a.Y == b.Y && a.Z == b.Z;
        }

        /// <summary>
        /// Determines two <see cref="Vertex"/> structs inequality.
        /// </summary>
        /// <param name="a">First <see cref="Vertex"/> to compare.</param>
        /// <param name="b">Second <see cref="Vertex"/> to compare.</param>
        /// <returns>True, if two provided <see cref="Vertex"/> structs are inequal, otherwise false.</returns>
        public static bool operator !=( Vertex a, Vertex b )
        {
            return !( a == b );
        }

        /// <summary>
        /// Determines one <see cref="Vertex"/> and other <see cref="Point3D"/> coordinates equality.
        /// </summary>
        /// <param name="a"><see cref="Vertex"/> object to compare.</param>
        /// <param name="b"><see cref="Point3D"/> object to compare.</param>
        /// <returns>True, if x,y and z coordinates of provided <see cref="Vertex"/> and <see cref="Point3D"/> objects are same, otherwise false.</returns>
        public static bool operator ==( Vertex a, Point3D b )
        {
            return a.X == b.X && a.Y == b.Y && a.Z == b.Z;
        }

        /// <summary>
        /// Determines one <see cref="Vertex"/> and other <see cref="Point3D"/> coordinates inequality.
        /// </summary>
        /// <param name="a"><see cref="Vertex"/> object to compare.</param>
        /// <param name="b"><see cref="Point3D"/> object to compare.</param>
        /// <returns>True, if x,y and z coordinates of provided <see cref="Vertex"/> and <see cref="Point3D"/> objects are different, otherwise false.</returns>
        public static Boolean operator !=( Vertex a, Point3D b )
        {
            return !( a == b );
        }

        /// <summary>
        /// Determines if provided <see cref="object"/> equals current <see cref="Vertex"/> struct.
        /// </summary>
        /// <param name="obj"><see cref="object"/> to compare with current <see cref="Vertex"/> struct.</param>
        /// <returns>True, if provided object equals current <see cref="Vertex"/> struct, otherwise false.</returns>
        public override bool Equals( object obj )
        {
            return obj != null && ( ( Vertex )obj ) == this;
        }

        /// <summary>
        /// Determines if provided <see cref="Vertex"/> struct equals current <see cref="Vertex"/> struct.
        /// </summary>
        /// <param name="other"><see cref="Vertex"/> struct to compare current to.</param>
        /// <returns>True, if provided <see cref="Vertex"/> struct equals current, otherwise false.</returns>
        public bool Equals( Vertex other )
        {
            return ( other != Invalid ) && ( X == other.X && Y == other.Y && Z == other.Z );
        }

        /// <summary>
        /// Gets current <see cref="Vertex"/> has code.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
