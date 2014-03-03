using System;
using System.Data;
using System.Data.SqlClient;

namespace L2.Net.DataProvider
{
    /// <summary>
    /// DataProvider MsSql connection class.
    /// </summary>
    internal sealed class MsSqlDataConnection : Connection
    {
        /// <summary>
        /// Temp hack to check if database is aviable.
        /// </summary>
        public const string dbValidation = "select * from sys.objects"; // where object_id = object_id(N'l2.net')";

        /// <summary>
        /// <see cref="SqlConnection"/> object.
        /// </summary>
        internal SqlConnection SqlServerConnection;

        /// <summary>
        /// Raises when current <see cref="MsSqlDataConnection"/> object connects to database engine.
        /// </summary>
        internal override event ConnectionConnectedEventDelegate OnConnected;
        /// <summary>
        /// Raises when current <see cref="MsSqlDataConnection"/> object disconnects to database engine.
        /// </summary>
        internal override event ConnectionDisconnectedEventDelegate OnDisconnected;

        /// <summary>
        /// Initializes new instance of <see cref="MsSqlDataConnection"/> object.
        /// </summary>
        public MsSqlDataConnection() : base() { }

        /// <summary>
        /// Initializes new instance of <see cref="MsSqlDataConnection"/> object.
        /// </summary>
        /// <param name="provider"><see cref="MsSqlDataProvider"/> that current <see cref="MsSqlDataConnection"/> is created by.</param>
        /// <param name="connectionID">Current <see cref="MsSqlDataConnection"/> unique id.</param>
        /// <param name="openOnInitialize">If true, opens connection to database engine just after initialization.</param>
        internal MsSqlDataConnection( MsSqlDataProvider provider, byte connectionID, bool openOnInitialize )
            : base(provider, connectionID)
        {
            SqlServerConnection = new SqlConnection(provider.ConnectionString);
            OnConnected += new ConnectionConnectedEventDelegate(provider.ConnectionConnected);
            OnDisconnected += new ConnectionDisconnectedEventDelegate(provider.ConnectionDisconnected);

            if ( openOnInitialize )
                Open();
        }

        /// <summary>
        /// Indicates if current <see cref="MsSqlDataConnection"/> is connected to database engine.
        /// </summary>
        internal override bool Connected
        {
            get { return SqlServerConnection != null && SqlServerConnection.State == ConnectionState.Open; }
        }

        /// <summary>
        /// Opens current <see cref="MsSqlDataConnection"/> database engine connection.
        /// </summary>
        internal override void Open()
        {
            if ( SqlServerConnection != null && SqlServerConnection.State != ConnectionState.Closed )
                InternalClose(true);

            InternalOpen();
        }

        /// <summary>
        /// Closes current <see cref="MsSqlDataConnection"/> database engine connection.
        /// </summary>
        /// <param name="reconnect"></param>
        internal override void Close( bool reconnect )
        {
            if ( SqlServerConnection != null && SqlServerConnection.State != ConnectionState.Closed )
                InternalClose(reconnect);
            else
                InternalOpen();
        }

        /// <summary>
        /// Closes connection to database engine.
        /// </summary>
        /// <param name="reconnect">If true, current <see cref="MsSqlDataConnection"/> object will attempt to reconnect to database engine.</param>
        protected internal override void InternalClose( bool reconnect )
        {
            lock ( this )
            {
                SqlServerConnection.Close();
                SqlServerConnection.Dispose();
                SqlServerConnection = null;

                if ( OnDisconnected != null )
                    OnDisconnected(ID, DisconnectType.OnShutdown);
            }

            if ( reconnect )
                InternalOpen();
        }

        /// <summary>
        /// Opens connection to database engine.
        /// </summary>
        protected internal override void InternalOpen()
        {
            lock ( this )
            {
                try
                {
                    SqlServerConnection = new SqlConnection(DBProvider.ConnectionString);
                    //SqlServerConnection.StatisticsEnabled = true;
                    SqlServerConnection.Open();

                    SqlCommand cmd = new SqlCommand(dbValidation, SqlServerConnection);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    if ( OnConnected != null )
                        OnConnected(this);
                }
                catch ( Exception e )
                {
                    Logger.Exception(e);
                }
            }
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="MsSqlDataConnection"/> and optionally releases the managed resources.
        /// </summary>
        /// <param name="dispose">True to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        public override void Dispose( bool dispose )
        {
            if ( !m_Disposed )
            {
                if ( dispose )
                {
                    lock ( this )
                    {
                        if ( SqlServerConnection != null )
                        {
                            if ( SqlServerConnection.State != ConnectionState.Closed )
                                InternalClose(false);
                        }
                    }
                }
            }
        }
    }
}