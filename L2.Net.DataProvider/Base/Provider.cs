using System;
using L2.Net.Network;
using L2.Net.Structs.Services;

namespace L2.Net.DataProvider
{
    /// <summary>
    /// Sql engine types.
    /// </summary>
    public enum SqlEngine : byte
    {
        /// <summary>
        /// MS SQL Server provider.
        /// </summary>
        MsSql = 0x00,
        /// <summary>
        /// MySql Server provider.
        /// </summary>
        MySql = 0x01,
        /// <summary>
        /// Odbc provider.
        /// </summary>
        Odbc = 0x02,
        /// <summary>
        /// Oledb provider.
        /// </summary>
        Oledb = 0x03,
        /// <summary>
        /// Oracle provider.
        /// </summary>
        Oracle = 0x04
    }
}

namespace L2.Net.DataProvider
{
    /// <summary>
    /// Data <see cref="Provider"/> initialized event delegate.
    /// </summary>
    /// <param name="type">Data <see cref="Provider"/> type.</param>
    /// <param name="activeConnectionsCount">Active connections count.</param>
    public delegate void ProviderInitializedEventDelegate( SqlEngine type, byte activeConnectionsCount );

    /// <summary>
    /// Data <see cref="Provider"/> stopped event delegate.
    /// </summary>
    public delegate void ProviderStoppedEventDelegate();

    /// <summary>
    /// Data provider abstract class.
    /// </summary>
    public abstract class Provider
    {
        #region Provider implementation

        /// <summary>
        /// Provider connection string.
        /// </summary>
        internal readonly string ConnectionString;

        /// <summary>
        /// Current <see cref="Provider"/> <see cref="SqlEngine"/> type.
        /// </summary>
        public readonly SqlEngine Type;

        /// <summary>
        /// Connections pool capacity (1 by default).
        /// </summary>
        public byte PoolSize = 1;

        /// <summary>
        /// Indicates if <see cref="Provider"/> is initialized.
        /// </summary>
        public bool Initialized;

        /// <summary>
        /// Raises after <see cref="Provider"/> is been initialized (all connections opened and ready).
        /// </summary>
        public abstract event ProviderInitializedEventDelegate OnInitialized;

        /// <summary>
        /// Raises after all active connections, used by current <see cref="Provider"/> was closed.
        /// </summary>
        public abstract event ProviderStoppedEventDelegate OnStopped;

        /// <summary>
        /// Interval in which database updates will execute.
        /// </summary>
        public TimeSpan QueueDumpInterval;

        /// <summary>
        /// Initializes new instance of <see cref="Provider"/> class.
        /// </summary>
        /// <param name="sqlType"><see cref="Provider"/> SQL type.</param>
        /// <param name="connectionString">Connection string.</param>
        /// <param name="queueDumpInterval">Interval in which lazy commands queue will execute collected commands.</param>
        public Provider( SqlEngine sqlType, string connectionString, TimeSpan queueDumpInterval )
        {
            ConnectionString = connectionString;
            Type = sqlType;
            QueueDumpInterval = queueDumpInterval;
        }

        /// <summary>
        /// Initializes current <see cref="Provider"/> object.
        /// </summary>
        public abstract void Initialize();

        /// <summary>
        /// Executes when some of current <see cref="Provider"/> <see cref="Connection"/> objects connects to database engine.
        /// </summary>
        /// <param name="connection">Connected <see cref="Connection"/>.</param>
        internal virtual void ConnectionConnected( Connection connection )
        {
            Logger.WriteLine(Source.DataProvider, "Connection {0} connected", connection.ID);
        }

        /// <summary>
        /// Executes when some of current <see cref="Provider"/> <see cref="Connection"/> objects disconnects from database engine.
        /// </summary>
        /// <param name="connectionID">Disconnected <see cref="Connection"/> unique id.</param>
        /// <param name="type">Disconnection type.</param>
        internal virtual void ConnectionDisconnected( byte connectionID, DisconnectType type )
        {
            Logger.WriteLine(Source.DataProvider, "Connection {0} disconnected {1}", connectionID, type);
        }

        #endregion

        #region Requests list

        /// <summary>
        /// Services audit record.
        /// </summary>
        /// <param name="serviceID">Service unique id.</param>
        /// <param name="serviceType">Service type.</param>
        /// <param name="auditType"><see cref="ServiceAuditType"/> parameter.</param>
        /// <param name="formatString">Format string.</param>
        /// <param name="args">Format string arguments.</param>
        public abstract void ServiceAudit( byte serviceID, byte serviceType, ServiceAuditType auditType, string formatString, params object[] args );

        /// <summary>
        /// Gets worlds list from database.
        /// </summary>
        /// <returns>Worlds list pre-cached data.</returns>
        public abstract WorldSummary[] Worlds_Cache();

        /// <summary>
        /// User authentication request processing.
        /// </summary>
        /// <param name="netRequest"><see cref="UserAuthenticationRequest"/> to verify user data from.</param>
        /// <param name="settings"><see cref="LoginServiceSettings"/> object.</param>
        /// <returns><see cref="UserAuthenticationResponse"/> struct.</returns>
        public abstract UserAuthenticationResponse User_Auth( UserAuthenticationRequest netRequest, LoginServiceSettings settings );

        /// <summary>
        /// User creation request processing.
        /// </summary>
        /// <param name="login">User login.</param>
        /// <param name="password">User password.</param>
        /// <param name="accessLevel">User account access level.</param>
        /// <returns>User account unique id.</returns>
        public abstract int User_Create( string login, string password, byte accessLevel );

        /// <summary>
        /// Updates user data after it's logout.
        /// </summary>
        /// <param name="uid">User unique id.</param>
        /// <param name="sessionStartTime">Session start time.</param>
        /// <param name="ip">Ip-address, user was using.</param>
        /// <param name="lastWorld">Last world, user played in.</param>
        public abstract void User_Logout( int uid, DateTime sessionStartTime, string ip, byte lastWorld );

        #endregion
    }
}