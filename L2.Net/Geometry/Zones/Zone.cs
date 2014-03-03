#define ZONE_REVALIDATION_COUNTER

using System.Collections.Generic;
using L2.Net.Geometry;
using L2.Net.Mobiles;

namespace L2.Net
{
    /// <summary>
    /// Represents method that will handle zone activation event.
    /// </summary>
    public delegate void ZoneActivatedEventHandler();
    /// <summary>
    /// Represents method that will handle zone de-activation event.
    /// </summary>
    public delegate void ZoneDeactivatedEventHandler();

    /// <summary>
    /// Represents method that will handle <see cref="Mobile"/> entering <see cref="Zone"/> event.
    /// </summary>
    /// <param name="m"></param>
    public delegate void ZoneEnterEventHandler( Mobile m );

    /// <summary>
    /// Represents method that will handle <see cref="Mobile"/> leaving <see cref="Zone"/> event.
    /// </summary>
    /// <param name="m"></param>
    public delegate void ZoneLeaveEventHandler( Mobile m );

    /// <summary>
    /// Abstract class for all world zones. (<see cref="ICompilable"/> interface must be implemented on on external scripts.)
    /// </summary>
    public abstract class Zone : IUniqueIdentified, INamed
    {
#if ZONE_REVALIDATION_COUNTER
        /// <summary>
        /// Debug counter value.
        /// </summary>
        protected long m_TimesInUse = 0;
#endif
        /// <summary>
        /// Collection of <see cref="IZone"/> objects, that current <see cref="Zone"/> contains.
        /// </summary>
        protected List<IZone> m_Zones;

        /// <summary>
        /// Current <see cref="Zone"/> unique identifier.
        /// </summary>
        protected int m_UniqueID;

        /// <summary>
        /// Current <see cref="Zone"/> name.
        /// </summary>
        protected string m_Name;

        /// <summary>
        /// Indicates if current <see cref="Zone"/> is active.
        /// </summary>
        protected bool m_IsActive;

        /// <summary>
        /// Occurs when current <see cref="Zone"/> was activated.
        /// </summary>
        protected abstract event ZoneActivatedEventHandler OnActivated;

        /// <summary>
        /// Occurs when current <see cref="Zone"/> was de-activated.
        /// </summary>
        protected abstract event ZoneDeactivatedEventHandler OnDeActivated;

        /// <summary>
        /// Occurs when some <see cref="Mobile"/> enters current <see cref="Zone"/>.
        /// </summary>
        protected abstract event ZoneEnterEventHandler OnEnter;

        /// <summary>
        /// Occurs when some <see cref="Mobile"/> leaves current <see cref="Zone"/>.
        /// </summary>
        protected abstract event ZoneLeaveEventHandler OnLeave;

        /// <summary>
        /// Initializes new instance of <see cref="Zone"/> class.
        /// </summary>
        /// <param name="zones">Array of <see cref="IZone"/> objects, that contain current <see cref="Zone"/>.</param>
        public Zone( params IZone[] zones )
        {
            m_Zones = new List<IZone>(zones);
        }

        /// <summary>
        /// Indicates if provided <see cref="Point3D"/> is inside current <see cref="Zone"/> geometrical space.
        /// </summary>
        /// <param name="p"><see cref="Point3D"/> object to validate.</param>
        /// <returns>True, if provided <see cref="Point3D"/> is inside current <see cref="Zone"/>, otherwise false.</returns>
        public virtual bool IsInside( Point3D p )
        {
#if ZONE_REVALIDATION_COUNTER
            IncrementRevalidationsCount();
#endif
            foreach ( IZone z in m_Zones )
                if ( z.IsInside(p) )
                    return true;
            return false;
        }

#if ZONE_REVALIDATION_COUNTER
        private void IncrementRevalidationsCount()
        {
            if ( m_TimesInUse + 1 != long.MaxValue )
                m_TimesInUse++;
            else
                m_TimesInUse = 1;
        }
#endif

        /// <summary>
        /// Adds provided <see cref="IZone"/> object to current <see cref="Zone"/> geometrical zones collection.
        /// </summary>
        /// <param name="zone"></param>
        protected void AddIZone( IZone zone )
        {
            m_Zones.Add(zone);
        }

        /// <summary>
        /// Clears currently containing <see cref="IZone"/> objects.
        /// </summary>
        public void ClearZones()
        {
            m_Zones.Clear();
        }

        /// <summary>
        /// Gets or sets value, indicating that current <see cref="Zone"/> is active.
        /// </summary>
        public virtual bool Active
        {
            get { return m_IsActive; }
            set { m_IsActive = value; }
        }

        /// <summary>
        /// Activates current <see cref="Zone"/>.
        /// </summary>
        public abstract void Activate();

        /// <summary>
        /// De-activates current <see cref="Zone"/>.
        /// </summary>
        public abstract void DeActivate();

        /// <summary>
        /// Handles <see cref="Mobile"/> <see cref="Zone"/> entering.
        /// </summary>
        /// <param name="m"><see cref="Mobile"/> that enters current <see cref="Zone"/>.</param>
        public abstract void Enter( Mobile m );

        /// <summary>
        /// Handles <see cref="Mobile"/> <see cref="Zone"/> leaving.
        /// </summary>
        /// <param name="m"><see cref="Mobile"/>, that leaves current <see cref="Zone"/>.</param>
        public abstract void Leave( Mobile m );

        /// <summary>
        /// Gets current <see cref="Zone"/> unique identifier.
        /// </summary>
        public int UniqueID
        {
            get { return m_UniqueID; }
            set { throw new System.InvalidOperationException("If you want to update Zone implementstion, uncache it first."); }
        }

        /// <summary>
        /// Gets or sets name of current <see cref="Zone"/>.
        /// </summary>
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }
    }
}
