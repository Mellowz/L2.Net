using System;

namespace L2.Net.Mobiles
{
    /// <summary>
    /// Represents the method that will handle <see cref="DefaultNpc.OnAIActivating"/> event.
    /// </summary>
    public delegate void AIActivatingEventHandler();
    /// <summary>
    /// Represents the method that will handle <see cref="DefaultNpc.OnAIDeactivating"/> event.
    /// </summary>
    public delegate void AIDeactivatingEventHandler();

    /// <summary>
    /// Base npc class.
    /// </summary>
    public abstract class DefaultNpc : Mobile
    {
        #region Events

        /// <summary>
        /// Occurs when <see cref="DefaultNpc"/> AI is activating.
        /// </summary>
        public virtual event AIActivatingEventHandler OnAIActivating;
        /// <summary>
        ///  Occurs when <see cref="DefaultNpc"/> AI is deactivating.
        /// </summary>
        public virtual event AIDeactivatingEventHandler OnAIDeactivating;

        #endregion

        /// <summary>
        /// Value, that indicates if current <see cref="DefaultNpc"/> AI is enabled.
        /// </summary>
        protected bool m_Active;

        /// <summary>
        /// Right hand item id.
        /// </summary>
        protected ushort m_RightHandItem;
        /// <summary>
        /// Chest item id.
        /// </summary>
        protected ushort m_ChestItem;
        /// <summary>
        /// Left hand item id.
        /// </summary>
        protected ushort m_LeftHandItem;

        /// <summary>
        /// Gets or sets right hand item id.
        /// </summary>
        public virtual ushort RightHandItem
        {
            get { return m_RightHandItem; }
            set { m_RightHandItem = value; }
        }

        /// <summary>
        /// Gets or sets chest item id.
        /// </summary>
        public virtual ushort ChestItem
        {
            get { return m_ChestItem; }
            set { m_ChestItem = value; }
        }

        /// <summary>
        /// Gets or sets left hand item id.
        /// </summary>
        public virtual ushort LeftHandItem
        {
            get { return m_LeftHandItem; }
            set { m_LeftHandItem = value; }
        }

        /// <summary>
        /// Gets or sets value, indicating if current <see cref="DefaultNpc"/> AI is active.
        /// </summary>
        public virtual bool Active
        {
            get { return m_Active; }
            set
            {
                if ( m_Active && !value && OnAIDeactivating != null )
                {
                    m_Active = false;
                    OnAIDeactivating();
                }

                if ( !m_Active && value && OnAIActivating != null )
                {
                    m_Active = true;
                    OnAIActivating();
                }
            }
        }

        public override bool MoveTo( Point3D p )
        {
            throw new NotImplementedException();
        }

        public override bool MoveTo( Mobile m )
        {
            throw new NotImplementedException();
        }

        public override bool MoveTo( Mobile m, double distance )
        {
            throw new NotImplementedException();
        }

        public override void StopMove()
        {
            throw new NotImplementedException();
        }

        public override void DoSocialAction( byte id )
        {
            base.DoSocialAction(id);
        }

        protected virtual void Think()
        {
        }

        protected virtual void BeginSearchTarget()
        {
        }

        protected virtual void EndSearchTarget( Mobile selected )
        {
        }
    }
}
