using System;

namespace L2.Net
{
    /// <summary>
    /// Geometry utilities.
    /// </summary>
    public static class Utility
    {
        /// <summary>
        /// Calculates distance between two <see cref="IPoint3D" /> objects.
        /// </summary>
        /// <param name="a">First <see cref="IPoint3D"/>.</param>
        /// <param name="b">Second <see cref="IPoint3D"/>.</param>
        /// <returns>Distance between two <see cref="IPoint3D" /> objects.</returns>
        public static double DistanceBetween( IPoint3D a, IPoint3D b )
        {
            return Math.Sqrt(( a.X - b.X ) * ( a.X - b.X ) + ( a.Y - b.Y ) * ( a.Y - b.Y ) + ( a.Z - b.Z ) * ( a.Z - b.Z ));
        }

        /// <summary>
        /// Calculates distance between two <see cref="Point3D" /> objects.
        /// </summary>
        /// <param name="a">First <see cref="Point3D"/>.</param>
        /// <param name="b">Second <see cref="Point3D"/>.</param>
        /// <returns>Distance between two <see cref="Point3D" /> objects.</returns>
        public static double DistanceBetween( Point3D a, Point3D b )
        {
            return Math.Sqrt(( a.X - b.X ) * ( a.X - b.X ) + ( a.Y - b.Y ) * ( a.Y - b.Y ) + ( a.Z - b.Z ) * ( a.Z - b.Z ));
        }

        /// <summary>
        /// Absolute value of <see cref="int"/> value.
        /// </summary>
        /// <param name="v"><see cref="int"/> value to validate.</param>
        /// <returns>Absolute value of <see cref="int"/> value.</returns>
        public static int AbsoluteValue( int v )
        {
            return v > 0 ? v : -v;
        }

        /// <summary>
        /// Absolute value of <see cref="float"/> value.
        /// </summary>
        /// <param name="v"><see cref="float"/> value to validate.</param>
        /// <returns>Absolute value of <see cref="float"/> value.</returns>
        public static float AbsoluteValue( float v )
        {
            return v > 0 ? v : -v;
        }
    }
}
