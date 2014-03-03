using System;

namespace L2.Net.DataProvider
{
    /// <summary>
    /// Database engine disconnect types.
    /// </summary>
    internal enum DisconnectType : byte
    {
        /// <summary>
        /// <see cref="Connection"/> was disconnected during shutdown (normally).
        /// </summary>
        OnShutdown = 0x00,

        /// <summary>
        /// <see cref="Connection"/> was disconnected because some <see cref="Exception"/> was thrown.
        /// </summary>
        OnException = 0x01
    }

    /// <summary>
    /// <see cref="Connection"/> object connected event delegate.
    /// </summary>
    /// <param name="connection"><see cref="Connection"/> object that just connected to database engine.</param>
    internal delegate void ConnectionConnectedEventDelegate( Connection connection );

    /// <summary>
    /// <see cref="Connection"/> object disconnected event delegate.
    /// </summary>
    /// <param name="connectionID">Disconnected <see cref="Connection"/> object unique id.</param>
    /// <param name="type">Disconnection type.</param>
    internal delegate void ConnectionDisconnectedEventDelegate( byte connectionID, DisconnectType type );

    /// <summary>
    /// Base database engine connection class.
    /// </summary>
    internal abstract class Connection : IDisposable
    {
        /// <summary>
        /// Current <see cref="Connection"/> object <see cref="Provider"/> reference.
        /// </summary>
        internal readonly Provider DBProvider;

        /// <summary>
        /// Current <see cref="Connection"/> object unique id.
        /// </summary>
        internal readonly byte ID;

        /// <summary>
        /// Dispose flag.
        /// </summary>
        protected bool m_Disposed = false;

        /// <summary>
        /// Raises after current <see cref="Connection"/> object connected to database engine.
        /// </summary>
        internal abstract event ConnectionConnectedEventDelegate OnConnected;

        /// <summary>
        /// Raises after current <see cref="Connection"/> object disconnected from database engine.
        /// </summary>
        internal abstract event ConnectionDisconnectedEventDelegate OnDisconnected;

        /// <summary>
        /// Indicates if current <see cref="Connection"/> is connected to database engine.
        /// </summary>
        internal abstract bool Connected { get; }

        /// <summary>
        /// Indicates if current <see cref="Connection"/> object is in idle state.
        /// </summary>
        internal volatile bool Idle = true;

        /// <summary>
        /// Initializes new instance of <see cref="Connection"/> object.
        /// </summary>
        internal Connection() { }

        /// <summary>
        /// Initializes new instance of <see cref="Connection"/> object.
        /// </summary>
        /// <param name="provider">Reference to current <see cref="Connection"/> <see cref="Provider"/>.</param>
        /// <param name="connectionID">Current <see cref="Connection"/> unique id.</param>
        internal Connection( Provider provider, byte connectionID )
        {
            DBProvider = provider;
            ID = connectionID;
        }

        /// <summary>
        /// Opens current <see cref="Connection"/>.
        /// </summary>
        internal abstract void Open();

        /// <summary>
        /// Closes current <see cref="Connection"/>.
        /// </summary>
        /// <param name="reconnect">True, to force current <see cref="Connection"/> to reconnect to database engine, otherwise false.</param>
        internal abstract void Close( bool reconnect );

        /// <summary>
        /// Not safe <see cref="Connection"/> Open() method analog.
        /// </summary>
        protected internal abstract void InternalOpen();

        /// <summary>
        /// Not safe <see cref="Connection"/> Close(bool reconnect) analog.
        /// </summary>
        /// <param name="reconnect">True, to force current <see cref="Connection"/> to reconnect to database engine, otherwise false.</param>
        protected internal abstract void InternalClose( bool reconnect );

        /// <summary>
        /// Releases all resources used by the <see cref="Connection"/>.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="Connection"/> and optionally releases the managed resources.
        /// </summary>
        /// <param name="dispose">True to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        public abstract void Dispose( bool dispose );
    }
}