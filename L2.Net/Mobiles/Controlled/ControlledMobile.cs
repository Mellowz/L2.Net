namespace L2.Net.Mobiles
{
    /// <summary>
    /// Abstract class for controlled mobiles (pet/summon).
    /// </summary>
    public abstract class ControlledMobile : Mobile, IControlled
    {
        /// <summary>
        /// <see cref="Actor"/>, that controls current <see cref="ControlledMobile"/>.
        /// </summary>
        private readonly Actor m_Master;

        /// <summary>
        /// Occurs when <see cref="Master"/> is under attack.
        /// </summary>
        public abstract event MasterAttackedEventHandler OnMasterAttacked;

        /// <summary>
        /// Occurs when <see cref="Master"/> calls <see cref="IControlled"/> mobile.
        /// </summary>
        public abstract event MasterCallEventHandler OnMasterCalls;

        /// <summary>
        /// Initializes new instance of <see cref="ControlledMobile"/>.
        /// </summary>
        /// <param name="owner">Owner of current <see cref="ControlledMobile"/> creature.</param>
        /// <param name="uiniqueID"><see cref="ControlledMobile"/> unique identifier.</param>
        public ControlledMobile( Actor owner, int uiniqueID )
            : base(uiniqueID)
        {
            m_Master = owner;
        }

        /// <summary>
        /// Provides access to current <see cref="ControlledMobile"/> master.
        /// </summary>
        public Actor Master
        {
            get { return m_Master; }
        }
    }
}
