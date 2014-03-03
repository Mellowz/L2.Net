using System;
using System.Collections.Generic;
using System.Linq;

namespace L2.Net.Geometry
{
    /// <summary>
    /// Represents <see cref="Polyhedron"/> struct.
    /// </summary>
    public struct Polyhedron : IMultiPlanedObject
    {

        /// <summary>
        /// Indicates if current <see cref="Polyhedron"/> was normalized yet.
        /// </summary>
        private bool m_Normalized;

        /// <summary>
        /// Indicates if current <see cref="Polyhedron"/> was triangulated yet.
        /// </summary>
        private bool m_Triangulated;

        /// <summary>
        /// Collection of <see cref="Vertex"/> objects, that geometrically are current <see cref="Polyhedron"/> vertexes.
        /// </summary>
        private readonly List<Vertex> Vertexes;

        /// <summary>
        /// Collection of <see cref="Tetrahedron"/> objects, that contain current <see cref="Polyhedron"/>.
        /// </summary>
        private List<Tetrahedron> Tetrahedrons;

        /// <summary>
        /// Initializes new instance of <see cref="Polyhedron"/> struct.
        /// </summary>
        /// <param name="vertexes">Array of <see cref="Vertex"/> objects to initialize from.</param>
        public Polyhedron( params Vertex[] vertexes )
        {
            if ( vertexes.Length < 4 )
                throw new InvalidOperationException("Can't initialize Polyhedron with less than 4 vertexes.");

            Vertexes = new List<Vertex>();
            Tetrahedrons = new List<Tetrahedron>();

            m_Normalized = true;
            m_Triangulated = true;

            Normalize();
            Triangulate();
        }

        /// <summary>
        /// Normalizes current <see cref="Polyhedron"/> object planes.
        /// </summary>
        public void Normalize()
        {
            if ( m_Normalized )
                return;

            //removing duplicates
            foreach ( Vertex v in Vertexes.ToArray() )
                if ( Vertexes.Contains(v) && Vertexes.Count(w => w == v) > 1 )
                    Vertexes.Remove(v);

            Vertexes.TrimExcess();
        }

        /// <summary>
        /// Triangulates current <see cref="Polyhedron"/>. 
        /// </summary>
        public void Triangulate()
        {
            if ( m_Triangulated )
                return;

            Tetrahedrons.Clear();

            for ( int i = 0; i < Vertexes.Count - 3; i++ )
                Tetrahedrons.Add(new Tetrahedron(new Vertex[] { Vertexes[i], Vertexes[i + 1], Vertexes[i + 2], Vertexes[i + 3] }));

            if ( Vertexes.Count - 4 > 0 )
            {
                Tetrahedrons.AddRange
                    (
                        new Tetrahedron[]
						{
							new Tetrahedron(	
											new Vertex[] { 
												Vertexes[Vertexes.Count - 1], Vertexes[0], Vertexes[1], Vertexes[2] }
											),
							new Tetrahedron(	
											new Vertex[] { 
												Vertexes[Vertexes.Count - 2], Vertexes[Vertexes.Count - 1], Vertexes[0], Vertexes[1] }
											),
							new Tetrahedron(	
											new Vertex[] { 
												Vertexes[Vertexes.Count - 3], Vertexes[Vertexes.Count - 2], Vertexes[Vertexes.Count - 1], Vertexes[0] }
											)
						}
                    );
            }
            else
                throw new InvalidOperationException("Can't initialize new instance of Polyhedron with less then 4 Vertexes.");
        }

        /// <summary>
        /// Indicates if provided <see cref="Point3D"/> is inside current <see cref="Polyhedron"/> object.
        /// </summary>
        /// <param name="p"><see cref="Point3D"/> to validate.</param>
        /// <returns>True, if provided <see cref="Point3D"/> is inside current <see cref="Polyhedron"/>, otherwise false.</returns>
        public bool IsInside( Point3D p )
        {
            foreach ( Tetrahedron t in Tetrahedrons )
                if ( t.IsInside(p) )
                    return true;

            return false;
        }
        
        /// <summary>
        /// Calculates distance between nearest face of current <see cref="Polyhedron"/> and provided <see cref="Point3D"/>.
        /// </summary>
        /// <param name="p"><see cref="Point3D"/> object to calculate distance to.</param>
        /// <returns>Distance between nearest face of current <see cref="Polyhedron"/> and provided <see cref="Point3D"/> object.</returns>
        public float DistanceTo( Point3D p )
        {
            return float.MaxValue; // not needed yet 
        }

        /// <summary>
        /// Indicates if current <see cref="Polyhedron"/> object was normalized yet.
        /// </summary>
        public bool Normalized
        {
            get { return m_Normalized; }
        }

        /// <summary>
        /// Indicates if current <see cref="Polyhedron"/> was triangulated yet.
        /// </summary>
        public bool Triangulated
        {
            get { return m_Triangulated; }
        }
    }
}
