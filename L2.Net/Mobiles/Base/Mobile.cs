using System;
using System.Timers;

namespace L2.Net.Mobiles
{
    /// <summary>
    /// Represents the method that will handle <see cref="Mobile.OnAppearing"/> event.
    /// </summary>
    public delegate void AppearingEventHandler();
    /// <summary>
    /// Represents the method that will handle <see cref="Mobile.OnDissapearing"/> event.
    /// </summary>
    public delegate void DisappearingEventHandler();
    /// <summary>
    /// Represents the method that will handle <see cref="Mobile.OnAggressiveAction"/> event.
    /// </summary>
    /// <param name="attacker">Attacked <see cref="Mobile"/>.</param>
    public delegate void AggressiveActionEventHandler( Mobile attacker );
    /// <summary>
    /// Represents the method that will handle <see cref="Mobile.OnDying"/> event.
    /// </summary>
    /// <param name="killer">Killer <see cref="Mobile"/>.</param>
    public delegate void DieEventHandler( Mobile killer );
    /// <summary>
    /// Represents the method that will handle <see cref="Mobile.OnResurrecting"/> event.
    /// </summary>
    public delegate void ResurrectEventHandler();
    /// <summary>
    /// Represents the method that will handle <see cref="Mobile.OnSpeedChanged"/> event.
    /// </summary>
    public delegate void SpeedChangedEventHandler();
    /// <summary>
    /// Represents the method that will handle <see cref="Mobile.OnLevelChanging"/> event.
    /// </summary>
    /// <param name="value">Positive or negative additional levels count.</param>
    public delegate void LevelChangingEventHandler( sbyte value );
    /// <summary>
    /// Represents the method that will handle <see cref="Mobile.OnFreezeLanding"/> event.
    /// </summary>
    public delegate void FreezeStateLandingEventHandler();
    /// <summary>
    /// Represents the method that will handle <see cref="Mobile.OnFreezeRemoving"/> event.
    /// </summary>
    public delegate void FreezeStateRemovingEventHandler();
    /// <summary>
    /// Represents the method that will handle <see cref="Mobile.OnNameChanged"/> event.
    /// </summary>
    public delegate void NameChangedEventHandler( string oldName, string newName );
    /// <summary>
    /// Represents the method that will handle <see cref="Mobile.OnTitleChanged"/> event.
    /// </summary>
    public delegate void TitleChangedEventHandler( string oldTitle, string newTitle );
    /// <summary>
    /// Represents the method that will handle <see cref="Mobile.OnBecomeRunning"/> event.
    /// </summary>
    public delegate void BecomeRunningEventHandler();
    /// <summary>
    /// Represents the method that will handle <see cref="Mobile.OnBecomeWalking"/> event.
    /// </summary>
    public delegate void BecomeWalkingEventHandler();
    /// <summary>
    /// Represents the method that will handle <see cref="Mobile.OnNameColorChanged"/> event.
    /// </summary>
    public delegate void NameColorChangedEventHandler( int newColor );
    /// <summary>
    /// Represents the method that will handle <see cref="Mobile.OnTitleColorChanged"/> event.
    /// </summary>
    public delegate void TitleColorChangedEventHandler( int newColor );

    /// <summary>
    /// Base class for all mobiles in L2 world.
    /// </summary>
    public abstract class Mobile : IMobile
    {
        #region Events

        /// <summary>
        /// Occurs when <see cref="Mobile"/> appears in the world (or teleports).
        /// </summary>
        public virtual event AppearingEventHandler OnAppearing;
        /// <summary>
        /// Occurs when <see cref="Mobile"/> disappears from the world (or teleports).
        /// </summary>
        public virtual event DisappearingEventHandler OnDissapearing;
        /// <summary>
        /// Occurs when some aggressive action is done over <see cref="Mobile"/>.
        /// </summary>
        public virtual event AggressiveActionEventHandler OnAggressiveAction;
        /// <summary>
        /// Occurs when <see cref="Mobile"/> dies.
        /// </summary>
        public virtual event DieEventHandler OnDying;
        /// <summary>
        /// Occurs when <see cref="Mobile"/> is resurrecting.
        /// </summary>
        public virtual event ResurrectEventHandler OnResurrecting;
        /// <summary>
        /// Occurs when <see cref="Mobile"/> speed changes.
        /// </summary>
        public virtual event SpeedChangedEventHandler OnSpeedChanged;
        /// <summary>
        /// Occurs when <see cref="Mobile"/> level is changing.
        /// </summary>
        public virtual event LevelChangingEventHandler OnLevelChanging;
        /// <summary>
        /// Occurs when <see cref="Mobile"/> enters freeze state. 
        /// </summary>
        public virtual event FreezeStateLandingEventHandler OnFreezeLanding;
        /// <summary>
        /// Occurs when <see cref="Mobile"/> leaves freeze state.
        /// </summary>
        public virtual event FreezeStateRemovingEventHandler OnFreezeRemoving;
        /// <summary>
        /// Occurs when <see cref="Mobile"/> name changes.
        /// </summary>
        public virtual event NameChangedEventHandler OnNameChanged;
        /// <summary>
        /// Occurs when <see cref="Mobile"/> title changes.
        /// </summary>
        public virtual event TitleChangedEventHandler OnTitleChanged;
        /// <summary>
        /// Occurs when <see cref="Mobile"/> name color changes.
        /// </summary>
        public virtual event NameColorChangedEventHandler OnNameColorChanged;
        /// <summary>
        /// Occurs when <see cref="Mobile"/> title color changes.
        /// </summary>
        public virtual event TitleColorChangedEventHandler OnTitleColorChanged;
        /// <summary>
        /// Occurs when <see cref="Mobile"/> goes to running mode.
        /// </summary>
        public virtual event BecomeRunningEventHandler OnBecomeRunning;
        /// <summary>
        /// Occurs when <see cref="Mobile"/> goes to walking mode.
        /// </summary>
        public virtual event BecomeWalkingEventHandler OnBecomeWalking;
        
        #endregion

        /// <summary>
        /// <see cref="Mobile"/> unique identifier.
        /// </summary>
        protected int m_UniqueID;
        /// <summary>
        /// <see cref="Mobile"/> health points value.
        /// </summary>
        protected float m_HP;
        /// <summary>
        /// <see cref="Mobile"/> max health points value.
        /// </summary>
        protected int m_MaxHP;
        /// <summary>
        /// <see cref="Mobile"/> mana points value.
        /// </summary>
        protected float m_MP;
        /// <summary>
        /// <see cref="Mobile"/> max mana points value.
        /// </summary>
        protected int m_MaxMP;
        /// <summary>
        /// Indicates if <see cref="Mobile"/> is auto-attackable.
        /// </summary>
        protected bool m_AutoAttackable;
        /// <summary>
        /// Indicates if <see cref="Mobile"/> is freezed.
        /// </summary>
        protected bool m_Freezed;
        /// <summary>
        /// Indicates if <see cref="Mobile"/> is visible.
        /// </summary>
        protected bool m_Visible;
        /// <summary>
        /// <see cref="Mobile"/> collision height.
        /// </summary>
        protected float m_CollisionHeight;
        /// <summary>
        /// <see cref="Mobile"/> collision radius.
        /// </summary>
        protected float m_CollisionRadius;
        /// <summary>
        /// <see cref="Mobile"/> level.
        /// </summary>
        protected sbyte m_Level;
        /// <summary>
        /// <see cref="Mobile"/> walking speed value.
        /// </summary>
        protected ushort m_WalkSpeed;
        /// <summary>
        /// <see cref="Mobile"/> running speed value.
        /// </summary>
        protected ushort m_RunSpeed;
        /// <summary>
        /// Current <see cref="Mobile"/> <see cref="Position"/> value.
        /// </summary>
        protected Position m_Position;
        /// <summary>
        /// <see cref="Mobile"/> name.
        /// </summary>
        protected string m_Name;
        /// <summary>
        /// <see cref="Mobile"/> title.
        /// </summary>
        protected string m_Title;
        /// <summary>
        /// <see cref="Mobile"/> name color value.
        /// </summary>
        protected int m_NameColor;
        /// <summary>
        /// <see cref="Mobile"/> title color value.
        /// </summary>
        protected int m_TitleColor;
        /// <summary>
        /// <see cref="Mobile"/> <see cref="MovingState"/> value.
        /// </summary>
        protected MovingState m_MovingType;
        /// <summary>
        /// <see cref="Mobile"/> health points regeneration value.
        /// </summary>
        protected float m_HPRegen;
        /// <summary>
        /// <see cref="Mobile"/> mana points regeneration value.
        /// </summary>
        protected float m_MPRegen;
        /// <summary>
        /// <see cref="Mobile"/> health points regeneration rate value.
        /// </summary>
        protected float m_HPRegenRate;
        /// <summary>
        /// <see cref="Mobile"/> mana points regeneration rate value.
        /// </summary>
        protected float m_MPRegenRate;
        /// <summary>
        /// <see cref="Mobile"/> experience value.
        /// </summary>
        protected ulong m_Exp;
        /// <summary>
        /// <see cref="Mobile"/> skill points value.
        /// </summary>
        protected uint m_SP;
        /// <summary>
        /// <see cref="Mobile"/> STR value.
        /// </summary>
        protected byte m_STR;
        /// <summary>
        /// <see cref="Mobile"/> CON value.
        /// </summary>
        protected byte m_CON;
        /// <summary>
        /// <see cref="Mobile"/> MEN value.
        /// </summary>
        protected byte m_MEN;
        /// <summary>
        /// <see cref="Mobile"/> DEX value.
        /// </summary>
        protected byte m_DEX;
        /// <summary>
        /// <see cref="Mobile"/> WIT value.
        /// </summary>
        protected byte m_WIT;
        /// <summary>
        /// <see cref="Mobile"/> INT value.
        /// </summary>
        protected byte m_INT;
        /// <summary>
        /// <see cref="Mobile"/> physical attack strength value
        /// </summary>
        protected ushort m_PAtk;
        /// <summary>
        /// <see cref="Mobile"/> magic attack strength value
        /// </summary>
        protected ushort m_MAtk;
        /// <summary>
        /// <see cref="Mobile"/> physical defense value.
        /// </summary>
        protected ushort m_PDef;
        /// <summary>
        /// <see cref="Mobile"/> magic defense value.
        /// </summary>
        protected ushort m_MDef;
        /// <summary>
        /// <see cref="Mobile"/> physical attack speed.
        /// </summary>
        protected ushort m_PAtkSpd;
        /// <summary>
        /// <see cref="Mobile"/> magic attack speed.
        /// </summary>
        protected ushort m_MAtkSpd;
        /// <summary>
        /// <see cref="Mobile"/> physical attack range.
        /// </summary>
        protected ushort m_PAtkRange;
        [Obsolete("This is skill dependent, so, must be removed later.")]
        protected ushort m_MAtkRange;

        /// <summary>
        /// General <see cref="Mobile"/> actions timer.
        /// </summary>
        protected Timer m_ActionTimer;


        /// <summary>
        /// Initializes new instance of <see cref="Mobile"/> object.
        /// </summary>
        public Mobile()
            : this(0)
        { }

        /// <summary>
        /// Initializes new instance of <see cref="Mobile"/> object.
        /// </summary>
        /// <param name="uniqueID"><see cref="Mobile"/> unique identifier.</param>
        public Mobile( int uniqueID )
        {
            m_UniqueID = uniqueID;
        }

        /// <summary>
        /// Gets or sets <see cref="Mobile"/> unique identifier.
        /// </summary>
        public int UniqueID
        {
            get { return m_UniqueID; }
            set { m_UniqueID = value; }
        }

        /// <summary>
        /// Gets or sets <see cref="Mobile"/> collision height.
        /// </summary>
        public float CollisionHeight
        {
            get { return m_CollisionHeight; }
            set { m_CollisionHeight = value; }
        }

        /// <summary>
        /// Gets or sets <see cref="Mobile"/> collision radius.
        /// </summary>
        public float CollisionRadius
        {
            get { return m_CollisionRadius; }
            set { m_CollisionRadius = value; }
        }

        /// <summary>
        /// Gets or sets <see cref="Mobile"/> <see cref="Position"/> value.
        /// </summary>
        public virtual Position Position
        {
            get { return m_Position; }
            set { m_Position = value; }
        }

        /// <summary>
        /// Gets or sets <see cref="Mobile"/> health points value.
        /// </summary>
        public virtual float HP
        {
            get { return m_HP; }
            set
            {
                if ( m_HP < 0 && OnDying != null )
                    OnDying(null);
                else if ( value > m_MaxHP )
                    m_HP = m_MaxHP;
                else
                    m_HP = value;
            }
        }

        /// <summary>
        /// Gets or sets <see cref="Mobile"/> max health points value.
        /// </summary>
        public virtual int MaxHP
        {
            get { return m_MaxHP; }
            set { m_MaxHP = value; }
        }

        /// <summary>
        /// Gets or sets <see cref="Mobile"/> mana points value.
        /// </summary>
        public virtual float MP
        {
            get { return m_MP; }
            set
            {
                if ( value < 0 )
                    m_MP = 0;
                else if ( value > m_MaxMP )
                    m_MP = m_MaxMP;
                else
                    m_MP = value;
            }
        }

        /// <summary>
        /// Gets or sets <see cref="Mobile"/> max mana points value.
        /// </summary>
        public virtual int MaxMP
        {
            get { return m_MaxMP; }
            set { m_MaxMP = value; }
        }

        /// <summary>
        /// Indicates if <see cref="Mobile"/> is alive.
        /// </summary>
        public virtual bool Alive
        {
            get { return m_HP > 0; }
            set
            {
                if ( value )
                {
                    if ( m_HP <= 0 )
                    {
                        m_HP = ( int )0.75 * m_MaxHP; // [TODO] - fix to proper value

                        if ( OnResurrecting != null )
                            OnResurrecting();
                    }
                }
                else
                {
                    m_HP = 0;

                    if ( OnDying != null )
                        OnDying(null);
                }
            }
        }

        /// <summary>
        /// Gets or sets value, indicating that current <see cref="Mobile"/> is auto-attackable.
        /// </summary>
        public virtual bool AutoAttackable
        {
            get { return m_AutoAttackable; }
            set { m_AutoAttackable = value; }
        }

        /// <summary>
        /// Gets or sets value, indicating if <see cref="Mobile"/> is currently freezed.
        /// </summary>
        public virtual bool Freezed
        {
            get { return m_Freezed; }
            set
            {
                if ( !m_Freezed && value && OnFreezeLanding != null )
                {
                    m_Freezed = true;
                    OnFreezeLanding();
                }

                if ( m_Freezed && !value && OnFreezeRemoving != null )
                {
                    m_Freezed = false;
                    OnFreezeRemoving();
                }
            }
        }

        /// <summary>
        /// Gets or sets value, indicating if <see cref="Mobile"/> is visible.
        /// </summary>
        public virtual bool Visible
        {
            get { return m_Visible; }
            set
            {
                m_Visible = value;

                if ( m_Visible && OnAppearing != null )
                    OnAppearing();
                else if ( OnDissapearing != null )
                    OnDissapearing();
            }
        }

        /// <summary>
        /// Gets or sets <see cref="Mobile"/> level value.
        /// </summary>
        public virtual sbyte Level
        {
            get { return m_Level; }
            set
            {
                if ( value != 0 && OnLevelChanging != null )
                    OnLevelChanging(value);
            }
        }

        /// <summary>
        /// Gets or sets <see cref="Mobile"/> running mode speed value.
        /// </summary>
        public virtual ushort RunSpeed
        {
            get { return m_RunSpeed; }
            set
            {
                m_RunSpeed = value;

                if ( OnSpeedChanged != null )
                    OnSpeedChanged();
            }
        }

        /// <summary>
        /// Gets or sets <see cref="Mobile"/> walking mode speed value.
        /// </summary>
        public virtual ushort WalkSpeed
        {
            get { return m_WalkSpeed; }
            set
            {
                m_WalkSpeed = value;

                if ( OnSpeedChanged != null )
                    OnSpeedChanged();
            }
        }

        /// <summary>
        /// Gets or sets <see cref="Mobile"/> name.
        /// </summary>
        public virtual string Name
        {
            get { return m_Name; }
            set
            {
                if ( value != null && !m_Name.Equals(value, StringComparison.InvariantCulture) )
                {
                    OnNameChanged(m_Name, value);
                    m_Name = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets <see cref="Mobile"/> title.
        /// </summary>
        public virtual string Title
        {
            get { return m_Title; }
            set
            {
                if ( value != null && !m_Title.Equals(value, StringComparison.InvariantCulture) )
                {
                    OnTitleChanged(m_Title, value);
                    m_Title = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets <see cref="Mobile"/> <see cref="MovingState"/> value.
        /// </summary>
        public virtual MovingState MovingState
        {
            get { return m_MovingType; }
            set
            {
                if ( m_MovingType != value )
                {
                    m_MovingType = value;

                    if ( m_MovingType == MovingState.Running && OnBecomeRunning != null )
                        OnBecomeRunning();
                    else if ( OnBecomeWalking != null )
                        OnBecomeWalking();
                }
            }
        }

        /// <summary>
        /// Gets or sets <see cref="Mobile"/> name color value.
        /// </summary>
        public virtual int NameColor
        {
            get { return m_NameColor; }
            set
            {
                m_NameColor = value;

                if ( OnNameColorChanged != null )
                    OnNameColorChanged(value);
            }
        }

        /// <summary>
        /// Gets or sets <see cref="Mobile"/> title color value.
        /// </summary>
        public virtual int TitleColor
        {
            get { return m_TitleColor; }
            set
            {
                m_TitleColor = value;

                if ( OnTitleColorChanged != null )
                    OnTitleColorChanged(value);
            }
        }

        /// <summary>
        /// Gets or sets <see cref="Mobile"/> health points regeneration value.
        /// </summary>
        public virtual float HPRegen
        {
            get { return m_HPRegen; }
            set { m_HPRegen = value; }
        }

        /// <summary>
        /// Gets or sets <see cref="Mobile"/> mana points regeneration value.
        /// </summary>
        public virtual float MPRegen
        {
            get { return m_MPRegen; }
            set { m_MPRegen = value; }
        }

        /// <summary>
        /// Gets or sets <see cref="Mobile"/> health points regeneration rate value.
        /// </summary>
        public virtual float HPRegenRate
        {
            get { return m_HPRegenRate; }
            set { m_HPRegenRate = value; }
        }

        /// <summary>
        /// Gets or sets <see cref="Mobile"/> mana points regeneration rate value.
        /// </summary>
        public virtual float MPRegenRate
        {
            get { return m_MPRegenRate; }
            set { m_MPRegenRate = value; }
        }

        /// <summary>
        /// Gets or sets <see cref="Mobile"/> STR value.
        /// </summary>
        public virtual byte STR
        {
            get { return m_STR; }
            set { m_STR = value; }
        }

        /// <summary>
        /// Gets or sets <see cref="Mobile"/> CON value.
        /// </summary>
        public virtual byte CON
        {
            get { return m_CON; }
            set { m_CON = value; }
        }

        /// <summary>
        /// Gets or sets <see cref="Mobile"/> MEN value.
        /// </summary>
        public virtual byte MEN
        {
            get { return m_MEN; }
            set { m_MEN = value; }
        }

        /// <summary>
        /// Gets or sets <see cref="Mobile"/> DEX value.
        /// </summary>
        public virtual byte DEX
        {
            get { return m_DEX; }
            set { m_DEX = value; }
        }

        /// <summary>
        /// Gets or sets <see cref="Mobile"/> WIT value.
        /// </summary>
        public virtual byte WIT
        {
            get { return m_WIT; }
            set { m_WIT = value; }
        }

        /// <summary>
        /// Gets or sets <see cref="Mobile"/> INT value.
        /// </summary>
        public virtual byte INT
        {
            get { return m_INT; }
            set { m_INT = value; }
        }

        /// <summary>
        /// Gets or sets <see cref="Mobile"/> experience value.
        /// </summary>
        public virtual ulong Exp
        {
            get { return m_Exp; }
            set { m_Exp = value; }
        }

        /// <summary>
        /// Gets or sets <see cref="Mobile"/> skill points value.
        /// </summary>
        public virtual uint SP
        {
            get { return m_SP; }
            set { m_SP = value; }
        }

        /// <summary>
        /// Gets or sets <see cref="Mobile"/> physical attack strength value.
        /// </summary>
        public virtual ushort PAtk
        {
            get { return m_PAtk; }
            set { m_PAtk = value; }
        }

        /// <summary>
        /// Gets or sets <see cref="Mobile"/> magic attack strength value.
        /// </summary>
        public virtual ushort MAtk
        {
            get { return m_MAtk; }
            set { m_MAtk = value; }
        }

        /// <summary>
        /// Gets or sets <see cref="Mobile"/> physical defense value.
        /// </summary>
        public virtual ushort PDef
        {
            get { return m_PDef; }
            set { m_PDef = value; }
        }

        /// <summary>
        /// Gets or sets <see cref="Mobile"/> magic defense value.
        /// </summary>
        public virtual ushort MDef
        {
            get { return m_MDef; }
            set { m_MDef = value; }
        }

        /// <summary>
        /// Gets or sets <see cref="Mobile"/> physical attack speed value.
        /// </summary>
        public virtual ushort PAtkSpd
        {
            get { return m_PAtkSpd; }
            set { m_PAtkSpd = value; }
        }

        /// <summary>
        /// Gets or sets <see cref="Mobile"/> magic attack speed value.
        /// </summary>
        public virtual ushort MAtkSpd
        {
            get { return m_MAtkSpd; }
            set { m_MAtkSpd = value; }
        }

        /// <summary>
        /// Gets or sets <see cref="Mobile"/> physical attack range.
        /// </summary>
        public ushort PAtkRange
        {
            get { return m_PAtkRange; }
            set { m_PAtkRange = value; }
        }

        [Obsolete("This is skill dependent, so, must be removed later.")]
        public ushort MAtkRange
        {
            get { return m_MAtkRange; }
            set { m_MAtkRange = value; }
        }





        public virtual bool MoveTo( Point3D p )
        {
            if ( !Alive )
                return false;

            return true;
        }

        public virtual bool MoveTo( Mobile m )
        {
            if ( !Alive )
                return false;

            return true;
        }

        public virtual bool MoveTo( Mobile m, double distance )
        {
            if ( !Alive )
                return false;

            return true;
        }

        public virtual void StopMove()
        {
            if ( !Alive )
                return;
        }

        public virtual void DoSocialAction( byte id )
        {
            if ( !Alive )
                return;
        }
    }
}