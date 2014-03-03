using System;
using System.Net;
using L2.Net.GameService.Properties;
using L2.Net.Network;
using L2.Net.Structs.Services;

namespace L2.Net.GameService.InnerNetwork
{
    /// <summary>
    /// Provides connection between cache service and game service.
    /// </summary>
    internal static class CacheServiceConnection
    {
        /// <summary>
        /// Indicates, if currently connected service has to reconnect to remote service automatically, when connection was lost.
        /// </summary>
        internal static volatile bool AutoReconnectToRemoteService = true;

        /// <summary>
        /// Cache server connection object.
        /// </summary>
        internal static InnerNetworkConnection Connection;

        /// <summary>
        /// Initializes service in unsafe mode.
        /// </summary>
        internal static void UnsafeInitialize()
        {
            if ( Connection != null )
            {
                if ( Connection.Connected )
                    throw new InvalidOperationException();

                Initialize(Connection.RemoteEndPoint, Connection.ReconnectInterval);

                return;
            }

            Service.Terminate(new ServiceShutdownEventArgs("Remote connection was not initialized yet"));
        }

        /// <summary>
        /// Initializes 'game to cache' connection.
        /// </summary>
        /// <param name="remoteEP">Remote cache service endpoint.</param>
        /// <param name="reconnectAttemptInterval">Interval between reconnection attempts.</param>
        internal static void Initialize( IPEndPoint remoteEP, TimeSpan reconnectAttemptInterval )
        {
            if ( Connection != null && Connection.Connected )
            {
                Logger.WriteLine(Source.InnerNetwork, "Already connected to remote service.");
                return;
            }

            if ( Connection == null )
            {
                Connection = new InnerNetworkConnection(remoteEP, reconnectAttemptInterval);
                Connection.HandleDelegate = CacheServiceRequestsHandlers.Handle;
                Connection.OnConnected += new OnConnectedEventHandler(Connection_OnConnected);
                Connection.OnDisconnected += new OnDisconnectedEventHandler(Connection_OnDisconnected);
            }
            else
            {
                Connection.RemoteEndPoint = remoteEP;
                Connection.ReconnectInterval = reconnectAttemptInterval;
            }

            Connection.BeginConnect();
        }

        /// <summary>
        /// Executes when connection to remote host has been aborted.
        /// </summary>
        /// <param name="errorCode">Error code.</param>
        /// <param name="client"><see cref="NetworkClient"/> object.</param>
        /// <param name="connectionId">Connection id.</param>
        private static void Connection_OnDisconnected( int errorCode, NetworkClient client, byte connectionId )
        {
            Logger.WriteLine(Source.InnerNetwork, "Disconnected from cache service.");

            if ( AutoReconnectToRemoteService )
            {
                Logger.WriteLine(Source.InnerNetwork, "Connection to remote side was lost, attempting to reconnect...");
                UnsafeInitialize();
            }
            else
                Service.Terminate(new ServiceShutdownEventArgs("Service terminated, can't operate without cache server connection."));
        }

        /// <summary>
        /// Executes when connection to remote host has been established.
        /// </summary>
        /// <param name="endPoint">Remote <see cref="IPEndPoint"/>.</param>
        /// <param name="connectionId">Connection id.</param>
        private static void Connection_OnConnected( IPEndPoint endPoint, byte connectionId )
        {
            Logger.WriteLine(Source.InnerNetwork, "Authorizing on CacheService on {0}", endPoint.ToString());

            Connection.Send
                (
                    new InitializeRequest(Settings.Default.ServiceUniqueID, ( byte )ServiceType.GameService).ToPacket()
                );

            Connection.BeginReceive();
        }

        /// <summary>
        /// Sends <see cref="Packet"/> to cache service.
        /// </summary>
        /// <param name="p"><see cref="Packet"/> to send.</param>
        internal static void Send( Packet p )
        {
            if ( Connection != null && Connection.Connected )
                Connection.Send(p);
            else
            {
                Logger.WriteLine(Source.InnerNetwork, "Failed to send packet to cache service, connection isn't active.");
                Logger.WriteLine(p.ToString());
            }
        }

        /// <summary>
        /// Indicates if logins service is connected to cache service.
        /// </summary>
        internal static bool Active
        {
            get
            {
                return Connection != null && Connection.Connected;
            }
        }
    }
}