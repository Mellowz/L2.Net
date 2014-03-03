using System.Net.Sockets;
using L2.Net.GameService.Properties;
using L2.Net.Network;
using L2.Net.Structs.Services;

namespace L2.Net.GameService.InnerNetwork
{
    /// <summary>
    /// Npc service connections manager.
    /// </summary>
    internal static class ConnectionsManager
    {
        /// <summary>
        /// Indicates if current game service has npc service opened connection(s).
        /// </summary>
        internal static volatile bool Active;

        /// <summary>
        /// Active npc service connection.
        /// </summary>
        private static InnerNetworkClient m_NpcServerConnection;

        /// <summary>
        /// Accepts incoming npc service connection.
        /// </summary>
        /// <param name="s">Accepted <see cref="Socket"/> object.</param>
        internal static void AcceptConnection( Socket s )
        {
            if ( s == null || !s.Connected )
                return;

            if ( !Active )
            {
                NetworkHelper.RemoteServiceInfo info = NetworkHelper.GetServiceInfo(s);

                if ( info.ServiceType == ServiceType.NpcService )
                {
                    m_NpcServerConnection = new InnerNetworkClient(info.ServiceId, info.ServiceType, s);

                    NpcServiceRequestsHandlers nsrh = new NpcServiceRequestsHandlers(ref m_NpcServerConnection);
                    //m_NpcServerConnection.OnDisconnected += new OnDisconnectedEventHandler(NpcServerConnection_OnDisconnected);
                    m_NpcServerConnection.HandleDelegate = nsrh.Handle;
                    m_NpcServerConnection.Send(new InitializeResponse(Settings.Default.ServiceUniqueID, ( byte )ServiceType.GameService, InitializeResponse.Accepted).ToPacket());

                    Active = true;

                    Logger.WriteLine(Source.InnerNetwork, "Connection accepted for {0} (0x{1})", info.ServiceType, info.ServiceId.ToString("x2"));

                }
            }
        }

        //private static void NpcServerConnection_OnDisconnected( int errorCode, NetworkClient client, byte connectionID )
        //{
        //    m_NpcServerConnection = null;
        //    NetworkListener.Enable();
        //}
    }
}