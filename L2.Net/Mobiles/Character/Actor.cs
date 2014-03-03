using System;
using System.Collections.Generic;

namespace L2.Net.Mobiles
{
    /// <summary>
    /// Represents the method that will handle <see cref="Actor.OnBeginEnterWorld"/> event.
    /// </summary>
    public delegate void BeginEnterWorldEventHandler();

    /// <summary>
    /// Represents the method that will handle <see cref="Actor.OnEndEnterWorld"/> event.
    /// </summary>
    public delegate void EndEnterWorldEventHandler();

    /// <summary>
    /// Represents the method that will handle <see cref="Actor.OnSittingDown"/> event.
    /// </summary>
    public delegate void BecomeSittingEventHandler();

    /// <summary>
    /// Represents the method that will handle <see cref="Actor.OnStandingUp"/> event.
    /// </summary>
    public delegate void BecomeStandingEventHandler();

    /// <summary>
    /// Represents the method that will handle <see cref="Actor.OnAppearanceChanged"/> event.
    /// </summary>
    public delegate void AppearanceChangedEventHandler();

    /// <summary>
    /// Represents the method that will handle <see cref="Actor.OnRecommendationsHaveValueChanged"/> event.
    /// </summary>
    /// <param name="newValue"></param>
    public delegate void RecommendationsHaveValueChangedEventHandler( ushort newValue );

    /// <summary>
    /// Represents the method that will handle <see cref="Actor.OnRecommendationsLeftValueChanged"/> event.
    /// </summary>
    /// <param name="newValue"></param>
    public delegate void RecommendationsLeftValueChangedEventHandler( ushort newValue );

    /// <summary>
    /// Actor class.
    /// </summary>
    public class Actor : Mobile, IMaster, IMobile
    {
        #region Events

        /// <summary>
        /// Occurs when <see cref="Actor"/> appears in the world (or teleports).
        /// </summary>
        public override event AppearingEventHandler OnAppearing;
        /// <summary>
        /// Occurs when <see cref="Actor"/> disappears from the world (or teleports).
        /// </summary>
        public override event DisappearingEventHandler OnDissapearing;
        /// <summary>
        /// Occurs when some aggressive action is done over <see cref="Mobile"/>.
        /// </summary>
        public override event AggressiveActionEventHandler OnAggressiveAction;
        /// <summary>
        /// Occurs when <see cref="Actor"/> dies.
        /// </summary>
        public override event DieEventHandler OnDying;
        /// <summary>
        /// Occurs when <see cref="Actor"/> is resurrecting.
        /// </summary>
        public override event ResurrectEventHandler OnResurrecting;
        /// <summary>
        /// Occurs when <see cref="Actor"/> speed changes.
        /// </summary>
        public override event SpeedChangedEventHandler OnSpeedChanged;
        /// <summary>
        /// Occurs when <see cref="Actor"/> level is changing.
        /// </summary>
        public override event LevelChangingEventHandler OnLevelChanging;
        /// <summary>
        /// Occurs when <see cref="Actor"/> enters freeze state. 
        /// </summary>
        public override event FreezeStateLandingEventHandler OnFreezeLanding;
        /// <summary>
        /// Occurs when <see cref="Actor"/> leaves freeze state.
        /// </summary>
        public override event FreezeStateRemovingEventHandler OnFreezeRemoving;
        /// <summary>
        /// Occurs when <see cref="Actor"/> name changes.
        /// </summary>
        public override event NameChangedEventHandler OnNameChanged;
        /// <summary>
        /// Occurs when <see cref="Actor"/> title changes.
        /// </summary>
        public override event TitleChangedEventHandler OnTitleChanged;
        /// <summary>
        /// Occurs when <see cref="Actor"/> name color changes.
        /// </summary>
        public override event NameColorChangedEventHandler OnNameColorChanged;
        /// <summary>
        /// Occurs when <see cref="Actor"/> title color changes.
        /// </summary>
        public override event TitleColorChangedEventHandler OnTitleColorChanged;
        /// <summary>
        /// Occurs when <see cref="Actor"/> goes to running mode.
        /// </summary>
        public override event BecomeRunningEventHandler OnBecomeRunning;
        /// <summary>
        /// Occurs when <see cref="Actor"/> goes to walking mode.
        /// </summary>
        public override event BecomeWalkingEventHandler OnBecomeWalking;
        /// <summary>
        /// Occurs when <see cref="Actor"/> begins enter into the world.
        /// </summary>
        public event BeginEnterWorldEventHandler OnBeginEnterWorld;
        /// <summary>
        /// Occurs when <see cref="Actor"/> ends enter into the world.
        /// </summary>
        public event EndEnterWorldEventHandler OnEndEnterWorld;
        /// <summary>
        /// Occurs when <see cref="Actor"/> sits on the ground.
        /// </summary>
        public event BecomeSittingEventHandler OnSittingDown;
        /// <summary>
        /// Occurs when <see cref="Actor"/> stands up.
        /// </summary>
        public event BecomeStandingEventHandler OnStandingUp;
        /// <summary>
        /// Occurs after something changes in actor's appearance.
        /// </summary>
        public event AppearanceChangedEventHandler OnAppearanceChanged;
        /// <summary>
        /// Occurs when amount of having recommendation changes. 
        /// </summary>
        public event RecommendationsHaveValueChangedEventHandler OnRecommendationsHaveValueChanged;
        /// <summary>
        /// Occurs when amount of left recommendations changes.
        /// </summary>
        public event RecommendationsLeftValueChangedEventHandler OnRecommendationsLeftValueChanged;

        #endregion

        /// <summary>
        /// <see cref="Actor"/> combat points value.
        /// </summary>
        private ushort m_CP;
        /// <summary>
        /// <see cref="Actor"/> max combat points value.
        /// </summary>
        private ushort m_MaxCP;
        /// <summary>
        /// <see cref="Actor"/> PK counter value.
        /// </summary>
        private ushort m_PK;
        /// <summary>
        /// <see cref="Actor"/> PVP counter value.
        /// </summary>
        private ushort m_PVP;
        /// <summary>
        /// <see cref="Actor"/> karma value.
        /// </summary>
        private ushort m_Karma;
        /// <summary>
        /// Amount of recommendations, that <see cref="Actor"/> currently have.
        /// </summary>
        private ushort m_RecommendationsHave;
        /// <summary>
        /// Amount of recommendations, that <see cref="Actor"/> can evaluate.
        /// </summary>
        private ushort m_RecommendationsLeft;
        /// <summary>
        /// <see cref="ControlledMobile"/> object (pet/summon).
        /// </summary>
        private ControlledMobile m_OwnedMobile;
        /// <summary>
        /// Gender of <see cref="Actor"/>.
        /// </summary>
        private Appearance m_Gender;
        /// <summary>
        /// Race of <see cref="Actor"/>.
        /// </summary>
        private Appearance m_Race;
        /// <summary>
        /// Hair color of <see cref="Actor"/>.
        /// </summary>
        private Appearance m_HairColor;
        /// <summary>
        /// Hair shape of <see cref="Actor"/>.
        /// </summary>
        private Appearance m_HairShape;
        /// <summary>
        /// Face shape of <see cref="Actor"/>.
        /// </summary>
        private Appearance m_FaceShape;
        /// <summary>
        /// Current <see cref="WaitingState"/>.
        /// </summary>
        private WaitingState m_WaitingState;

        /// <summary>
        /// <see cref="DateTime"/> where is stored last time <see cref="Actor"/> data was sent to cache.
        /// </summary>
        public DateTime LastSavedTime;

        /// <summary>
        /// Delegate to method, that can save <see cref="Actor"/> immediately.
        /// </summary>
        public Delegate SaveMeDelegate;

        /// <summary>
        /// Gets or sets <see cref="Actor"/> combat points value.
        /// </summary>
        public ushort CP
        {
            get { return m_CP; }
            set { m_CP = value; }
        }

        /// <summary>
        /// Gets or sets <see cref="Actor"/> max combat points value.
        /// </summary>
        public ushort MaxCP
        {
            get { return m_MaxCP; }
            set { m_MaxCP = value; }
        }

        /// <summary>
        /// Gets or sets <see cref="Actor"/> PK counter value.
        /// </summary>
        public ushort PK
        {
            get { return m_PK; }
            set { m_PK = value; }
        }

        /// <summary>
        /// Gets or sets <see cref="Actor"/> PVP counter value.
        /// </summary>
        public ushort PvP
        {
            get { return m_PVP; }
            set { m_PVP = value; }
        }

        /// <summary>
        /// Gets or sets <see cref="Actor"/> karma value.
        /// </summary>
        public ushort Karma
        {
            get { return m_Karma; }
            set { m_Karma = value; }
        }

        /// <summary>
        /// Gets or sets amount of recommendations, that <see cref="Actor"/> have.
        /// </summary>
        public ushort RecomendationsHave
        {
            get { return m_RecommendationsHave; }
            set
            {
                m_RecommendationsHave = value;

                if ( OnRecommendationsHaveValueChanged != null )
                    OnRecommendationsHaveValueChanged(value);
            }
        }

        /// <summary>
        /// Gets or sets amount of recommendations, currently aviable for <see cref="Actor"/>.
        /// </summary>
        public ushort RecomendationsLeft
        {
            get { return m_RecommendationsLeft; }
            set
            {
                m_RecommendationsLeft = value;

                if ( OnRecommendationsLeftValueChanged != null )
                    OnRecommendationsLeftValueChanged(value);
            }
        }

        /// <summary>
        /// Gets or sets value, indicating <see cref="Actor"/> <see cref="WaitingState"/>.
        /// </summary>
        public WaitingState WaitingState
        {
            get { return m_WaitingState; }
            set
            {
                if ( m_WaitingState != value )
                {
                    m_WaitingState = value;

                    if ( m_WaitingState == WaitingState.Sitting && OnSittingDown != null )
                        OnSittingDown();
                    else if ( OnStandingUp != null )
                        OnStandingUp();
                }
            }
        }

        /// <summary>
        /// Gets or sets <see cref="Actor"/> gender.
        /// </summary>
        public Appearance Gender
        {
            get { return m_Gender; }
            set
            {
                if ( value != m_Gender && OnAppearanceChanged != null )
                {
                    m_Gender = value;
                    OnAppearanceChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets <see cref="Actor"/> race.
        /// </summary>
        public Appearance Race
        {
            get { return m_Race; }
            set
            {
                if ( value != m_Race && OnAppearanceChanged != null )
                {
                    m_Race = value;
                    OnAppearanceChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets <see cref="Actor"/> hair color index.
        /// </summary>
        public Appearance HairColor
        {
            get { return m_HairColor; }
            set
            {
                if ( value != m_HairColor && OnAppearanceChanged != null )
                {
                    m_HairColor = value;
                    OnAppearanceChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets <see cref="Actor"/> hair shape index.
        /// </summary>
        public Appearance HairShape
        {
            get { return m_HairShape; }
            set
            {
                if ( value != m_HairShape && OnAppearanceChanged != null )
                {
                    m_HairShape = value;
                    OnAppearanceChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets <see cref="Actor"/> face shape index.
        /// </summary>
        public Appearance FaceShape
        {
            get { return m_FaceShape; }
            set
            {
                if ( value != m_FaceShape && OnAppearanceChanged != null )
                {
                    m_FaceShape = value;
                    OnAppearanceChanged();
                }
            }
        }

        /// <summary>
        /// Provides access to <see cref="ControlledMobile"/>, that <see cref="Actor"/> owns.
        /// </summary>
        public ControlledMobile ControlledMobile
        {
            get { return m_OwnedMobile; }
        }

        public override bool MoveTo( Point3D p )
        {
            return base.MoveTo(p);
        }

        public override bool MoveTo( Mobile m )
        {
            return base.MoveTo(m);
        }

        public override bool MoveTo( Mobile m, double distance )
        {
            return base.MoveTo(m, distance);
        }

        public override void StopMove()
        {
            base.StopMove();
        }

        public override void DoSocialAction( byte id )
        {
            base.DoSocialAction(id);
        }
    }
}