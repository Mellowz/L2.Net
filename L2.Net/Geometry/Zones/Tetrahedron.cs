#define DEBUG_TETRAHEDRON

using System;
using System.Collections.Generic;
using System.Linq;

namespace L2.Net.Geometry
{
    /// <summary>
    /// <see cref="Tetrahedron"/> struct representation.
    /// </summary>
    public struct Tetrahedron : IMultiPlanedObject
    {
        /// <summary>
        /// Calculations constant.
        /// </summary>
        internal static readonly float fConst = ( float )Math.Pow(2, ( 3d / 2d ));

        /// <summary>
        /// Indicates if current <see cref="Tetrahedron"/> was normalized yet.
        /// </summary>
        private bool m_Normalized;

        /// <summary>
        /// Indicates if current <see cref="Tetrahedron"/> was triangulated yet.
        /// </summary>
        private bool m_Triangulated;

        /// <summary>
        /// Collection of <see cref="Plane"/> objects, that geometrically contain current <see cref="Tetrahedron"/> object.
        /// </summary>
        private readonly List<Plane> Planes;

        /// <summary>
        /// Collection of <see cref="Vertex"/> objects, that geometrically are current <see cref="Tetrahedron"/> vertexes.
        /// </summary>
        private readonly List<Vertex> Vertexes;

        /// <summary>
        /// Initializes new instance if <see cref="Tetrahedron"/> struct.
        /// </summary>
        /// <param name="vertexes">Array of 4 <see cref="Vertex"/> objects to initialize from.</param>
        public Tetrahedron( params Vertex[] vertexes )
        {
            if ( vertexes.Length != 4 )
                throw new ArgumentOutOfRangeException("vertexes", "Tetrahedron initialization Vertexes count must be 4.");


            Vertexes = new List<Vertex>();
            Planes = new List<Plane>();

            Vertexes.AddRange(vertexes);

            Planes = new List<Plane>
                (
                    new Plane[]
						{
							new Plane(Vertexes[0], Vertexes[1], Vertexes[2]),
							new Plane(Vertexes[1], Vertexes[2], Vertexes[3]),
							new Plane(Vertexes[0], Vertexes[2], Vertexes[3]),
							new Plane(Vertexes[0], Vertexes[1], Vertexes[3])
						}
                );

            m_Normalized = true;
            m_Triangulated = true; // really there is no need to triangulate, but...
            Normalize(); // let's validate
        }

        /// <summary>
        /// Normalizes current <see cref="Tetrahedron"/> object vertexes. Don't call this method externally.
        /// </summary>
        public void Normalize()
        {
            if ( m_Normalized )
                return;

            //removing duplicates
            foreach ( Vertex v in Vertexes.ToArray() )
                if ( Vertexes.Contains(v) && Vertexes.Count(w => w == v) > 1 ) // this is not the best way to compare, but i believe, that there is no need to re-set zones in 'on-the-fly' mode, it will be too expansive operation to re-validate all objects inside/outside all zones.
                    Vertexes.Remove(v);

            if ( Vertexes.Count < 4 )
                throw new InvalidOperationException("Tetrahedron vertexes can't be same points in space.");

            Vertexes.TrimExcess();
        }

        /// <summary>
        /// Triangulates current <see cref="Tetrahedron"/> object. Don't call this method externally.
        /// </summary>
        public void Triangulate()
        {
            if ( m_Triangulated )
                return;
        }

        /// <summary>
        /// Calculates distance between nearest face of current <see cref="Tetrahedron"/> and provided <see cref="Point3D"/>.
        /// </summary>
        /// <param name="p"><see cref="Point3D"/> object to calculate distance to.</param>
        /// <returns>Distance between nearest face of current <see cref="Tetrahedron"/> and provided <see cref="Point3D"/> object.</returns>
        public float DistanceTo( Point3D p )
        {
            return float.MaxValue; // not needed yet 
        }

        /// <summary>
        /// <para>Validates, if provided <see cref="Point3D"/> is inside current <see cref="Tetrahedron"/>.</para>
        /// <para>Comments:</para>
        /// <para>I found theoreme, about point and tetrahedron, when point is inside it, so i'll try to translate. I think there is better solution, but i was lazy to find it. So, for now i'll keep this method in debug state.</para>
        /// <para>[eng]</para>
        /// <para>For any tetrahedron and point inside it: sum of distances between point and tetrahedron faces and is at least Math.Pow(2, (3d/2d)) times less, then sum of distances between point and tetrahedron vertexes.</para>
        /// <para>[rus] (Теорема Эрдеша-Морделла-Барроу / http://www.mccme.ru/mmmf-lectures/books/index.php?task=archive&year=2003&sem=2)</para>
        /// <para>Для любого тетраэдра и точки, расположенной внутри него, сумма расстояний от этой точки до граней тетраэдра не менее чем в 2 в степени (3/2) раз меньше суммы расстояний от этой точки до вершин тетраэдра.</para>
        /// </summary>
        /// <param name="p"><see cref="Point3D"/> which position must be validated.</param>
        /// <returns>True, if provided <see cref="Point3D"/> is inside current <see cref="Sphere"/>, otherwise false.</returns>
        public bool IsInside( Point3D p )
        {
            foreach ( Vertex v in Vertexes ) // same point
                if ( v == p )
                    return true;
#if DEBUG_TETRAHEDRON
            //distance to vertexes & edges
            float dv = 0f, de = 0f;

            foreach ( Vertex v in Vertexes )
                dv += v.DistanceTo(p);

            foreach ( Plane pl in Planes )
                de += pl.DistanceTo(p);

            float check = dv / fConst;

            bool res = de < check;

            return res;
#else
            return false; // as you wish, i prefere false :)
#endif
        }

        /// <summary>
        /// Indicates if current <see cref="Tetrahedron"/> was normalized yet.
        /// </summary>
        public bool Normalized
        {
            get { return m_Normalized; }
        }
        /// <summary>
        /// Indicates if current <see cref="Tetrahedron"/> was triangulated yet.
        /// </summary>
        public bool Triangulated
        {
            get { return m_Triangulated; }
        }
    }
}
