using System;
using System.Collections.Generic;
using System.Net.Sockets;
using L2.Net.CacheService.Network.Handlers;
using L2.Net.CacheService.Properties;
using L2.Net.Network;
using L2.Net.Structs.Services;

namespace L2.Net.CacheService.Network
{
    /// <summary>
    /// Provides methods to operate over inner network connections.
    /// </summary>
    internal static class ConnectionsManager
    {
        /// <summary>
        /// Aviable connections list.
        /// </summary>
        private static readonly SortedList<byte, InnerNetworkClient> m_ActiveConnections = new SortedList<byte, InnerNetworkClient>();

        /// <summary>
        /// Pre-cached initialization response.
        /// </summary>
        private static readonly Packet m_ResponseAccepted = new InitializeResponse
            (
                Settings.Default.ServiceUniqueID,
                ( byte )ServiceType.CacheService,
                InitializeResponse.Accepted
            ).ToPacket();

        /// <summary>
        /// Pre-cached initialization response.
        /// </summary>
        private static readonly Packet m_ResponseRejected = new InitializeResponse
            (
                Settings.Default.ServiceUniqueID,
                ( byte )ServiceType.CacheService,
                InitializeResponse.Rejected
             ).ToPacket();

        /// <summary>
        /// Serves for connections acceptance.
        /// </summary>
        /// <param name="socket">Accepted <see cref="Socket"/>.</param>
        internal static void AcceptConnection( Socket socket )
        {
            if ( socket == null || !socket.Connected )
                return;

            NetworkHelper.RemoteServiceInfo info = NetworkHelper.GetServiceInfo(socket);

            if ( info.ServiceType == ServiceType.Undefined )
            {
                Console.WriteLine("Connection rejected for remote connection from {0}, service was not recognized.", socket.RemoteEndPoint);
                NetworkHelper.CloseSocket(ref socket);
                return;
            }

            if ( m_ActiveConnections.ContainsKey(info.ServiceId) )
            {
                Console.WriteLine("{0} with id 0x{1} already connected, skipping connection request.", info.ServiceType, info.ServiceId);
                NetworkHelper.CloseSocket(ref socket);
                return;
            }

            InnerNetworkClient client = null;

            switch ( info.ServiceType )
            {
                case ServiceType.LoginService:
                    {
                        client = new InnerNetworkClient(info.ServiceId, info.ServiceType, socket);
                        client.OnDisconnected += new OnDisconnectedEventHandler(OnRemoteConnectionError);
                        client.HandleDelegate = new LoginServiceRequestsHandlers(ref client).Handle;

                        break;
                    }
                case ServiceType.GameService:
                    {
                        client = new InnerNetworkClient(info.ServiceId, info.ServiceType, socket);
                        client.OnDisconnected += new OnDisconnectedEventHandler(OnRemoteConnectionError);
                        client.HandleDelegate = new GameServiceRequestsHandlers(ref client).Handle;
                        break;
                    }
                case ServiceType.NpcService:
                    {
                        client = new InnerNetworkClient(info.ServiceId, info.ServiceType, socket, NpcServiceRequestsHandlers.Handle);
                        break;
                    }
                default:
                    {
                        throw new InvalidOperationException();
                    }
            }

            Logger.WriteLine(Source.InnerNetwork, "Connection accepted for {0} (0x{1})", client.ServiceType, client.ServiceID.ToString("x2"));
            client.Send(m_ResponseAccepted);
            client.BeginReceive();
            m_ActiveConnections.Add(info.ServiceId, client);
        }

        /// <summary>
        /// Occurs when client to one of active <see cref="InnerNetworkClient"/>s brakes up.
        /// </summary>
        /// <param name="errorCode">Exception code, if aviable.</param>
        /// <param name="client">Disconnected <see cref="NetworkClient"/> object.</param>
        /// <param name="connectionId">Connection id.</param>
        private static void OnRemoteConnectionError( int errorCode, NetworkClient client, byte connectionId )
        {
            InnerNetworkClient disconnected = ( InnerNetworkClient )client;

            if ( disconnected != null )
            {
                switch ( errorCode )
                {
                    case 10054: // An existing client was forcibly closed by the remote host
                        {
                            Logger.WriteLine(Source.InnerNetwork, "Remote {0} (0x{1}) closed current connection it self.", disconnected.ServiceType, disconnected.ServiceID.ToString("x2"));
                            break;
                        }
                    default:
                        {
                            Logger.WriteLine(Source.InnerNetwork, "Remote {0} (0x{1}) closed current connection, error code: {2}.", disconnected.ServiceType, disconnected.ServiceID.ToString("x2"), errorCode);
                            break;
                        }
                }

                CloseConnection(disconnected.ServiceID);
            }
        }

        /// <summary>
        /// Closes existing client to remote service.
        /// </summary>
        /// <param name="remoteServiceId">Remote service unique id.</param>
        internal static void CloseConnection( byte remoteServiceId )
        {
            if ( m_ActiveConnections != null && m_ActiveConnections.ContainsKey(remoteServiceId) )
            {
                InnerNetworkClient client = m_ActiveConnections[remoteServiceId];

                if ( client != null )
                {
                    if ( client.Connected )
                        client.CloseConnection();

                    client = null;
                }

                m_ActiveConnections.Remove(remoteServiceId);
            }
        }

        /// <summary>
        /// Sets <see cref="RemoteServiceSettings"/> to specified service.
        /// </summary>
        /// <param name="settings"><see cref="RemoteServiceSettings"/> object to set as settings.</param>
        internal static void SetServiceSettings( RemoteServiceSettings settings )
        {
            if ( settings != null && m_ActiveConnections.ContainsKey(settings.ServiceUniqueID) )
            {
                InnerNetworkClient client = m_ActiveConnections[settings.ServiceUniqueID];
                client.RemoteServiceSettings = settings;
                client.Send
                    (
                        new SetSettingsResponse(SetSettingsResponse.Accepted).ToPacket()
                    );
                m_ActiveConnections[settings.ServiceUniqueID] = client;
                Logger.WriteLine(Source.InnerNetwork, "{0} (0x{1}) settings update done.", client.ServiceType, client.ServiceID.ToString("x2"));
            }
        }
    }
}