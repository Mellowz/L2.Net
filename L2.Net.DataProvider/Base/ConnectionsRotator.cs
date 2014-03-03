using System.Collections.Generic;

namespace L2.Net.DataProvider
{
    /// <summary>
    /// <see cref="Connection"/> objects rotator interface.
    /// </summary>
    /// <typeparam name="C"><see cref="Connection"/> object <see cref="System.Type"/>.</typeparam>
    internal interface IConnectionsRotator<C> where C : Connection, new()
    {
        /// <summary>
        /// Enqueues specified <see cref="Connection"/> object to current <see cref="IConnectionsRotator&lt;C&gt;"/> queue.
        /// </summary>
        /// <param name="connection"><see cref="Connection"/> object to enqueue.</param>
        /// <param name="identity"><see cref="Connection"/> object unique id.</param>
        void Enqueue( C connection, byte identity );

        /// <summary>
        /// Gets next <see cref="Connection"/> object from current <see cref="IConnectionsRotator&lt;C&gt;"/> queue.
        /// </summary>
        /// <returns>Next <see cref="Connection"/> object.</returns>
        C Next();

        /// <summary>
        /// Gets amount of active <see cref="Connection"/> objects in <see cref="IConnectionsRotator&lt;C&gt;"/> queue.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Clears <see cref="Connection"/> objects queue and unlocks current <see cref="IConnectionsRotator&lt;C&gt;"/> object.
        /// </summary>
        /// <returns>Currently active <see cref="Connection"/> objects.</returns>
        C[] Release();

        /// <summary>
        /// Gets or sets value, that indicates if current <see cref="IConnectionsRotator&lt;C&gt;"/> is locked.
        /// </summary>
        bool Locked { get; set; }

        /// <summary>
        /// Indicates if current <see cref="IConnectionsRotator&lt;C&gt;"/> queue contains <see cref="Connection"/> with specified unique id.
        /// </summary>
        /// <param name="identity">Unique id to verify.</param>
        /// <returns>True, if current <see cref="IConnectionsRotator&lt;C&gt;"/> queue contains <see cref="Connection"/> with specified unique id, otherwise false.</returns>
        bool Contains( byte identity );

        /// <summary>
        /// Removes <see cref="Connection"/> object from <see cref="IConnectionsRotator&lt;C&gt;"/> queue.
        /// </summary>
        /// <param name="identity"><see cref="Connection"/> object unique id.</param>
        void Remove( byte identity );
    }

    /// <summary>
    /// <see cref="Connection"/> objects rotator class.
    /// </summary>
    /// <typeparam name="C"><see cref="Connection"/> object <see cref="System.Type"/>.</typeparam>
    internal sealed class ConnectionsRotator<C> : IConnectionsRotator<C> where C : Connection, new()
    {
        /// <summary>
        /// Lock object.
        /// </summary>
        private readonly object m_Lock = new object();

        /// <summary>
        /// Active <see cref="Connection"/> objects queue.
        /// </summary>
        private readonly Queue<C> m_ActiveConnections;

        /// <summary>
        /// Connections identity collection.
        /// </summary>
        private readonly List<byte> m_ConnectionsIdentity;

        /// <summary>
        /// Indicates if current <see cref="ConnectionsRotator&lt;C&gt; "/> is locked now.
        /// </summary>
        private bool m_Locked;

        /// <summary>
        /// Initializes new instance of <see cref="ConnectionsRotator&lt;C&gt; "/> class.
        /// </summary>
        /// <param name="poolSize">Default <see cref="Connection"/> objects queue capacity.</param>
        internal ConnectionsRotator( int poolSize )
        {
            lock ( m_Lock )
            {
                m_ActiveConnections = new Queue<C>(poolSize);
                m_ConnectionsIdentity = new List<byte>();
            }
        }

        /// <summary>
        /// Enqueues specified <see cref="Connection"/> object to current <see cref="ConnectionsRotator&lt;C&gt; "/> queue if it's not locked.
        /// </summary>
        /// <param name="connection"><see cref="Connection"/> object to enqueue.</param>
        /// <param name="identity"><see cref="Connection"/> object unique id.</param>
        public void Enqueue( C connection, byte identity )
        {
            if ( !m_Locked )
            {
                lock ( m_Lock )
                {
                    m_ActiveConnections.Enqueue(connection);
                    m_ConnectionsIdentity.Add(identity);
                }
            }
        }

        /// <summary>
        /// Gets next <see cref="Connection"/> object from current <see cref="ConnectionsRotator&lt;C&gt; "/> queue.
        /// </summary>
        /// <returns>Next <see cref="Connection"/> object.</returns>
        public C Next()
        {
            lock ( m_Lock )
            {
                C next = m_ActiveConnections.Dequeue();
                m_ActiveConnections.Enqueue(next);
                return next.Idle ? next : Next();
            }
        }

        /// <summary>
        /// Gets amount of active <see cref="Connection"/> objects in <see cref="ConnectionsRotator&lt;C&gt; "/> queue.
        /// </summary>
        public int Count
        {
            get
            {
                lock ( m_Lock )
                    return m_ActiveConnections.Count;
            }
        }

        /// <summary>
        /// Clears <see cref="Connection"/> objects queue and unlocks current <see cref="ConnectionsRotator&lt;C&gt; "/> object.
        /// </summary>
        /// <returns>Currently active <see cref="Connection"/> objects.</returns>
        public C[] Release()
        {
            lock ( m_Lock )
            {
                C[] dest = new C[m_ActiveConnections.Count];
                m_ActiveConnections.CopyTo(dest, 0);
                m_ActiveConnections.Clear();
                m_Locked = false;
                return dest;
            }
        }

        /// <summary>
        /// Gets or sets value, that indicates if current <see cref="ConnectionsRotator&lt;C&gt; "/> is locked.
        /// </summary>
        public bool Locked
        {
            get { return m_Locked; }
            set { m_Locked = value; }
        }

        /// <summary>
        /// Indicates if current <see cref="ConnectionsRotator&lt;C&gt; "/> queue contains <see cref="Connection"/> with specified unique id.
        /// </summary>
        /// <param name="identity">Unique id to verify.</param>
        /// <returns>True, if current <see cref="ConnectionsRotator&lt;C&gt; "/> queue contains <see cref="Connection"/> with specified unique id, otherwise false.</returns>
        public bool Contains( byte identity )
        {
            lock ( m_Lock )
                return m_ConnectionsIdentity.Contains(identity);
        }

        /// <summary>
        /// Removes <see cref="Connection"/> object from current <see cref="ConnectionsRotator&lt;C&gt; "/> queue.
        /// </summary>
        /// <param name="identity"><see cref="Connection"/> object unique id.</param>
        public void Remove( byte identity )
        {
            if ( m_ConnectionsIdentity.Contains(identity) )
            {
                lock ( m_Lock )
                {
                    m_ConnectionsIdentity.Clear();

                    foreach ( C connection in Release() )
                    {
                        Connection intern = connection as Connection;

                        if ( intern != null && intern.ID != identity )
                            Enqueue(connection, intern.ID);
                        else
                            intern.Dispose();

                        m_Locked = true;
                    }
                }
            }
        }
    }
}