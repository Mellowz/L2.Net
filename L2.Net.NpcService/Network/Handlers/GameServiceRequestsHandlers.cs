using System;
using L2.Net.Structs.Services;
using L2.Net.Network;

namespace L2.Net.NpcService.Network
{
    /// <summary>
    /// Game service requests handlers.
    /// </summary>
    internal sealed class GameServiceRequestsHandlers
    {
        /// <summary>
        /// Referenced <see cref="InnerNetworkConnection"/> object.
        /// </summary>
        internal InnerNetworkConnection Connection;

        /// <summary>
        /// Handler execution thread id.
        /// </summary>
        internal byte ConnectionID;

        /// <summary>
        /// Initializes new instance of <see cref="GameServiceRequestsHandlers"/> class.
        /// </summary>
        /// <param name="connection">Referenced <see cref="InnerNetworkConnection"/> object.</param>
        /// <param name="connectionID">Connection id.</param>
        internal GameServiceRequestsHandlers( ref InnerNetworkConnection connection, byte connectionID )
        {
            Connection = connection;
            ConnectionID = connectionID;
        }

        /// <summary>
        /// Handles incoming <see cref="Packet"/> objects.
        /// </summary>
        /// <param name="p"><see cref="Packet"/> to handle.</param>
        internal void Handle( Packet p )
        {
            Logger.WriteLine("Received {0}", p.ToString());

            switch ( p.FirstOpcode )
            {
                case ServiceLayer.Identity:
                    {
                        switch ( p.SecondOpcode )
                        {
                            case ServiceLayer.InitializeRequest:
                                {
                                    return; // login service not handles incoming connections.
                                }
                            case ServiceLayer.InitializeResponse: // connection init response
                                {
                                    InitializeResponse data = new InitializeResponse(p);

                                    switch ( data.Answer )
                                    {
                                        case InitializeResponse.Accepted:
                                            {
                                                Logger.WriteLine(Source.InnerNetwork, "Connected to {0} (0x{1})", GameServiceConnection.Connection.ServiceType, GameServiceConnection.Connection.ServiceID.ToString("x2"));
                                                return;
                                            }
                                        case InitializeResponse.Rejected:
                                            {
                                                Logger.WriteLine(Source.InnerNetwork, "Connection rejected by remote service {0} (0x{1})", ( ServiceType )data.RemoteServiceType, data.RemoteServiceID);
                                                return;
                                            }
                                    }

                                    return;
                                }
                        }

                        break;
                    }
            }

            Logger.WriteLine("Unknown packet received from {0} service on layer 0x{1}:{2}{3}", ServiceType.GameService, p.FirstOpcode.ToString("x2"), Environment.NewLine, p.ToString());
        }
    }
}