namespace L2.Net.Mobiles
{
    /// <summary>
    /// Represents the method that will handle <see cref="Citizen.OnTalk"/> event.
    /// </summary>
    /// <param name="requester"></param>
    public delegate void TalkingEventHandler( Actor requester );

    /// <summary>
    /// Base class for citizen npcs type.
    /// </summary>
    public class Citizen : DefaultNpc
    {
        #region Events

        /// <summary>
        /// Occurs when some <see cref="Actor"/> talks to current <see cref="Citizen"/> mobile object.
        /// </summary>
        public event TalkingEventHandler OnTalk;
        /// <summary>
        /// Occurs when <see cref="Citizen"/> appears in the world (or teleports).
        /// </summary>
        public override event AppearingEventHandler OnAppearing;
        /// <summary>
        /// Occurs when <see cref="Citizen"/> disappears from the world (or teleports).
        /// </summary>
        public override event DisappearingEventHandler OnDissapearing;
        /// <summary>
        /// Occurs when some aggressive action is done over <see cref="Citizen"/>.
        /// </summary>
        public override event AggressiveActionEventHandler OnAggressiveAction;
        /// <summary>
        /// Occurs when <see cref="Citizen"/> dies.
        /// </summary>
        public override event DieEventHandler OnDying;
        /// <summary>
        /// Occurs when <see cref="Citizen"/> is resurrecting.
        /// </summary>
        public override event ResurrectEventHandler OnResurrecting;
        /// <summary>
        /// Occurs when <see cref="Citizen"/> speed changes.
        /// </summary>
        public override event SpeedChangedEventHandler OnSpeedChanged;
        /// <summary>
        /// Occurs when <see cref="Citizen"/> level is changing.
        /// </summary>
        public override event LevelChangingEventHandler OnLevelChanging;
        /// <summary>
        /// Occurs when <see cref="Citizen"/> enters freeze state. 
        /// </summary>
        public override event FreezeStateLandingEventHandler OnFreezeLanding;
        /// <summary>
        /// Occurs when <see cref="Citizen"/> leaves freeze state.
        /// </summary>
        public override event FreezeStateRemovingEventHandler OnFreezeRemoving;
        /// <summary>
        /// Occurs when <see cref="Citizen"/> name changes.
        /// </summary>
        public override event NameChangedEventHandler OnNameChanged;
        /// <summary>
        /// Occurs when <see cref="Citizen"/> title changes.
        /// </summary>
        public override event TitleChangedEventHandler OnTitleChanged;
        /// <summary>
        /// Occurs when <see cref="Citizen"/> name color changes.
        /// </summary>
        public override event NameColorChangedEventHandler OnNameColorChanged;
        /// <summary>
        /// Occurs when <see cref="Citizen"/> title color changes.
        /// </summary>
        public override event TitleColorChangedEventHandler OnTitleColorChanged;
        /// <summary>
        /// Occurs when <see cref="Citizen"/> goes to running mode.
        /// </summary>
        public override event BecomeRunningEventHandler OnBecomeRunning;
        /// <summary>
        /// Occurs when <see cref="Citizen"/> goes to walking mode.
        /// </summary>
        public override event BecomeWalkingEventHandler OnBecomeWalking;

        #endregion

        public Citizen()
        {
        }
    }
}