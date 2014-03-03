using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using L2.Net.Network;
using L2.Net.Structs.Services;

namespace L2.Net.DataProvider
{
    /// <summary>
    /// Microsoft SQL Server data provider class.
    /// </summary>
    public sealed class MsSqlDataProvider : Provider
    {
        #region Provider implementation

        /// <summary>
        /// Raises after <see cref="MsSqlDataProvider"/> was initialized (all connections are open).
        /// </summary>
        public override event ProviderInitializedEventDelegate OnInitialized;

        /// <summary>
        /// Raises when <see cref="MsSqlDataProvider"/> has not any active <see cref="MsSqlDataConnection"/>.
        /// </summary>
        public override event ProviderStoppedEventDelegate OnStopped;

        /// <summary>
        /// Active <see cref="MsSqlDataConnection"/> objects rotator.
        /// </summary>
        private ConnectionsRotator<MsSqlDataConnection> m_ActiveConnections;

        /// <summary>
        /// Lazy cache commands queue.
        /// </summary>
        private LazyCommandsQueue<MsSqlDataCommand> m_CommandsQueue;

        /// <summary>
        /// Initializes new instance of <see cref="MsSqlDataProvider"/> class.
        /// </summary>
        /// <param name="connectionString">Database engine connection string.</param>
        /// <param name="poolSize"><see cref="MsSqlDataConnection"/> objects that <see cref="MsSqlDataProvider"/> will use.</param>
        /// <param name="queueDumpInterval">Lazy <see cref="Command"/> objects queue dump interval.</param>
        public MsSqlDataProvider( string connectionString, byte poolSize, TimeSpan queueDumpInterval )
            : base(SqlEngine.MsSql, connectionString, queueDumpInterval)
        {
            PoolSize = poolSize;
            m_ActiveConnections = new ConnectionsRotator<MsSqlDataConnection>(poolSize);
            m_CommandsQueue = new LazyCommandsQueue<MsSqlDataCommand>(QueueDumpInterval);
            m_CommandsQueue.DumpEventHandler += new LazyQueueDumpEventHandler<MsSqlDataCommand>(LazyQueue_DumpHandler);
        }

        /// <summary>
        /// Executes when lazy queue dumps actual <see cref="Command"/> objects collection.
        /// </summary>
        /// <param name="queue"><see cref="MsSqlDataCommand"/> objects to execute.</param>
        private void LazyQueue_DumpHandler( MsSqlDataCommand[] queue )
        {
            Logger.WriteLine(Source.DataProvider, "Running LazyCommandsQueue dump, {0} commands in queue", queue.Length);

            if ( queue.Length > 0 )
                new Thread(new ParameterizedThreadStart(LazyQueue_DumpHandlerThread)) { IsBackground = false, Priority = ThreadPriority.BelowNormal }.Start(queue);
        }

        /// <summary>
        /// Start lazy cache dump <see cref="Thread"/>.
        /// </summary>
        /// <param name="q"><see cref="MsSqlDataCommand"/> objects array.</param>
        private void LazyQueue_DumpHandlerThread( object q )
        {
            MsSqlDataConnection connection = new MsSqlDataConnection(this, byte.MaxValue, true);

            MsSqlDataCommand[] queue = ( MsSqlDataCommand[] )q;

            for ( int i = 0; i < queue.Length; i++ )
            {
                MsSqlDataCommand msdc = queue[i];

                SetConnection(ref msdc, connection);

                msdc.ExecuteNonQuery();

                ReleaseCommand(msdc);
            }

            queue = null;

            connection.Close(false);
            connection.Dispose();
            connection = null;
        }

        /// <summary>
        /// Initializes current <see cref="MsSqlDataProvider"/> object.
        /// </summary>
        public override void Initialize()
        {
            Initialized = false;

            if ( m_ActiveConnections.Count > 0 )
                foreach ( MsSqlDataConnection connection in m_ActiveConnections.Release() )
                    connection.Dispose();

            for ( byte i = 0; i < PoolSize && i < byte.MaxValue - 1; i++ )
                new MsSqlDataConnection(this, i, true);
        }

        /// <summary>
        /// Executes when some of current <see cref="MsSqlDataProvider"/> <see cref="MsSqlDataConnection"/> objects connects to database engine.
        /// </summary>
        /// <param name="connection">Connected <see cref="MsSqlDataConnection"/>.</param>
        internal override void ConnectionConnected( Connection connection )
        {
            if ( connection.ID == byte.MaxValue )
                return;

            if ( !m_ActiveConnections.Contains(connection.ID) )
            {
                m_ActiveConnections.Enqueue(( MsSqlDataConnection )connection, connection.ID);

                if ( m_ActiveConnections.Count == PoolSize )
                {
                    Initialized = true;
                    m_ActiveConnections.Locked = true;

                    if ( OnInitialized != null )
                        OnInitialized(SqlEngine.MsSql, ( byte )m_ActiveConnections.Count);
                }
            }
            else
                throw new InvalidOperationException("Provider already has connection with provided id.");
        }

        /// <summary>
        /// Executes when some of current <see cref="MsSqlDataProvider"/> <see cref="MsSqlDataConnection"/> objects disconnects from database engine.
        /// </summary>
        /// <param name="connectionID">Disconnected <see cref="MsSqlDataConnection"/> unique id.</param>
        /// <param name="type">Disconnection type.</param>
        internal override void ConnectionDisconnected( byte connectionID, DisconnectType type )
        {
            if ( m_ActiveConnections.Contains(connectionID) )
            {
                Logger.WriteLine(Source.DataProvider, "Connection {0} was disconnected {1}.", connectionID, type);
                m_ActiveConnections.Remove(connectionID);

                if ( m_ActiveConnections.Count == 0 )
                {
                    Initialized = false;

                    if ( OnStopped != null )
                        OnStopped();
                }
            }
        }
        #endregion

        #region Commands execution implementation

        /// <summary>
        /// Releases provided <see cref="MsSqlDataCommand"/> resources.
        /// </summary>
        /// <param name="msdc"><see cref="MsSqlDataCommand"/> object to release.</param>
        private static void ReleaseCommand( MsSqlDataCommand msdc )
        {
            if ( msdc != null )
            {
                msdc.Dispose();
                msdc = null;
            }
        }

        /// <summary>
        /// Sets <see cref="MsSqlDataConnection"/> reference to provided <see cref="MsSqlDataCommand"/> object and prepares it.
        /// </summary>
        /// <param name="msdc"><see cref="MsSqlDataCommand"/> object.</param>
        /// <param name="connection"><see cref="MsSqlDataConnection"/> to reference.</param>
        private static void SetConnection( ref MsSqlDataCommand msdc, MsSqlDataConnection connection )
        {
            msdc.Prepare(ref connection);
        }

        /// <summary>
        /// Services audit record.
        /// </summary>
        /// <param name="serviceID">Service unique id.</param>
        /// <param name="serviceType">Service type.</param>
        /// <param name="auditType"><see cref="ServiceAuditType"/> parameter.</param>
        /// <param name="formatString">Format string.</param>
        /// <param name="args">Format string arguments.</param>
        public override void ServiceAudit( byte serviceID, byte serviceType, ServiceAuditType auditType, string formatString, params object[] args )
        {
            m_CommandsQueue.Enqueue
                (
                    new MsSqlDataCommand
                    (
                        "[Service_Audit]",
                        CommandType.StoredProcedure,
                        new SqlParameter("@service_id", SqlDbType.TinyInt) { Value = serviceID },
                        new SqlParameter("@service_type", SqlDbType.TinyInt) { Value = serviceType },
                        new SqlParameter("@action", SqlDbType.TinyInt) { Value = auditType },
                        new SqlParameter("@args", SqlDbType.Text) { Value = String.IsNullOrEmpty(formatString) ? String.Empty : String.Format(formatString, args) }
                    )
           );
        }

        /// <summary>
        /// Gets worlds list from database.
        /// </summary>
        /// <returns>Worlds list pre-cached data.</returns>
        public override WorldSummary[] Worlds_Cache()
        {
            MsSqlDataCommand msdc = new MsSqlDataCommand
                (
                    "[Worlds_Cache]",
                    CommandType.StoredProcedure
                );

            SetConnection(ref msdc, m_ActiveConnections.Next());
            DataTable worlds = new DataTable();

            WorldSummary[] array = new WorldSummary[0] { };

            if ( msdc.Execute(ref worlds) && worlds.Rows.Count > 0 )
            {
                array = new WorldSummary[worlds.Rows.Count];

                for ( int i = 0; i < array.Length; i++ )
                {
                    DataRow row = worlds.Rows[i];

                    array[i] = new WorldSummary()
                    {
                        ID = ( byte )row["id"],
                        Address = System.Net.IPAddress.Parse(( string )row["outer_ip"]).GetAddressBytes(),
                        Port = ( int )row["port"],
                        AccessLevel = ( byte )row["access_level"],
                        UsersMax = ( short )row["max_users"],
                        AgeLimit = ( byte )row["age_limit"],
                        IsPvP = ( bool )row["is_pvp"],
                        IsTestServer = ( bool )row["is_test"],
                        ShowClock = ( bool )row["show_clock"],
                        ShowBrackets = ( bool )row["show_brackets"]
                    };
                }
            }

            ReleaseCommand(msdc);

            return array;
        }

        /// <summary>
        /// User authentication request processing.
        /// </summary>
        /// <param name="netRequest"><see cref="UserAuthenticationRequest"/> to verify user data from.</param>
        /// <param name="settings"><see cref="LoginServiceSettings"/> object.</param>
        /// <returns><see cref="UserAuthenticationResponse"/> struct.</returns>
        public override UserAuthenticationResponse User_Auth( UserAuthenticationRequest netRequest, LoginServiceSettings settings )
        {
            MsSqlDataCommand msdc = new MsSqlDataCommand
            (
                "[User_Auth]",
                CommandType.StoredProcedure,
                new SqlParameter("@login", SqlDbType.VarChar, 0x10) { Value = netRequest.Login },
                new SqlParameter("@password", SqlDbType.VarChar, 0x2f) { Value = netRequest.Password },
                new SqlParameter("@uid", SqlDbType.Int) { Direction = ParameterDirection.Output },
                new SqlParameter("@last_world", SqlDbType.TinyInt) { Direction = ParameterDirection.Output },
                new SqlParameter("@access_level", SqlDbType.TinyInt) { Direction = ParameterDirection.Output }
            );

            SetConnection(ref msdc, m_ActiveConnections.Next());

            msdc.ExecuteNonQuery();

            int uid = TypesConverter.GetInt(msdc.Parameters["@uid"].Value, int.MinValue);

            UserAuthenticationResponse rsp = new UserAuthenticationResponse(netRequest.RequestID, UserAuthenticationResponseType.UserOrPasswordWrong);

            switch ( uid )
            {
                case int.MinValue: // conversion error
                    {
                        rsp.Response = UserAuthenticationResponseType.SystemError;
                        break;
                    }
                case -2: // invalid credentials 
                    break;
                case -1: // login doesn't exist
                    {
                        if ( settings != null && settings.AutoCreateUser )
                        {
                            // creating user
                            uid = User_Create(netRequest.Login, netRequest.Password, settings.DefaultAccessLevel);

                            switch ( uid )
                            {
                                case int.MinValue: // conversion error
                                case -2: // db insert error
                                    {
                                        rsp.Response = UserAuthenticationResponseType.SystemError;
                                        break;
                                    }
                                case -1: // login already exists
                                    break;
                                default: // user created
                                    {
                                        rsp.Response = UserAuthenticationResponseType.UserAccepted;
                                        rsp.UserID = uid;
                                        rsp.AccessLevel = settings.DefaultAccessLevel;
                                        rsp.LastWorldID = TypesConverter.GetByte(msdc.Parameters["@last_world"].Value, 1);
                                        break;
                                    }
                            }
                        }
                        else
                            goto case -2;
                        break;
                    }
                default:
                    {
                        rsp.Response = UserAuthenticationResponseType.UserAccepted;
                        rsp.UserID = uid;
                        rsp.LastWorldID = TypesConverter.GetByte(msdc.Parameters["@last_world"].Value, 1);
                        rsp.AccessLevel = TypesConverter.GetByte(msdc.Parameters["@access_level"].Value, 0);
                        break;
                    }
            }

            ReleaseCommand(msdc);

            return rsp;
        }

        /// <summary>
        /// User creation request processing.
        /// </summary>
        /// <param name="login">User login.</param>
        /// <param name="password">User password.</param>
        /// <param name="accessLevel">User account access level.</param>
        /// <returns>User account unique id.</returns>
        public override int User_Create( string login, string password, byte accessLevel )
        {
            MsSqlDataCommand msdc = new MsSqlDataCommand
                (
                    "[User_Create]",
                     CommandType.StoredProcedure,
                     new SqlParameter("@login", SqlDbType.VarChar, 0x10) { Value = login },
                     new SqlParameter("@password", SqlDbType.VarChar, 0x2f) { Value = password },
                     new SqlParameter("@access_level", SqlDbType.TinyInt) { Value = accessLevel },
                     new SqlParameter("@uid", SqlDbType.Int) { Direction = ParameterDirection.Output }
                );

            SetConnection(ref msdc, m_ActiveConnections.Next());
            msdc.ExecuteNonQuery();

            int uid = TypesConverter.GetInt(msdc.Parameters["@uid"].Value, int.MinValue);

            ReleaseCommand(msdc);

            return uid;
        }

        /// <summary>
        /// Updates user data after it's logout.
        /// </summary>
        /// <param name="uid">User unique id.</param>
        /// <param name="sessionStartTime">Session start time.</param>
        /// <param name="ip">Ip-address, user was using.</param>
        /// <param name="lastWorld">Last world, user played in.</param>
        public override void User_Logout( int uid, DateTime sessionStartTime, string ip, byte lastWorld )
        {
            m_CommandsQueue.Enqueue
                (
                    new MsSqlDataCommand
                        (
                            "[User_Logout]",
                            CommandType.StoredProcedure,
                            new SqlParameter("@uid", SqlDbType.Int) { Value = uid },
                            new SqlParameter("@session_start_time", SqlDbType.DateTime) { Value = sessionStartTime },
                            new SqlParameter("@used_time", SqlDbType.BigInt) { Value = ( DateTime.Now - sessionStartTime ).Ticks },
                            new SqlParameter("@last_world", SqlDbType.TinyInt) { Value = lastWorld == 0 ? 1 : lastWorld },
                            new SqlParameter("@last_ip", SqlDbType.VarChar) { Value = ip }
                        )
                );
        }
        #endregion
    }
}