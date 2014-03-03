using System;
using L2.Net.Network;
using L2.Net.Structs.Services;

namespace L2.Net.CacheService.Network.Handlers
{
    /// <summary>
    /// Game service requests handler.
    /// </summary>
    internal sealed class GameServiceRequestsHandlers
    {
        /// <summary>
        /// Inner network client for incoming packets handling.
        /// </summary>
        internal InnerNetworkClient Service;

        /// <summary>
        /// Initializes new instance of <see cref="GameServiceRequestsHandlers"/> class.
        /// </summary>
        /// <param name="client">Referenced <see cref="InnerNetworkClient"/> object.</param>
        internal GameServiceRequestsHandlers( ref InnerNetworkClient client )
        {
            Service = client;
        }

        /// <summary>
        /// Sends <see cref="Packet"/> to connected service.
        /// </summary>
        /// <param name="p"><see cref="Packet"/> to send.</param>
        internal void Send( Packet p )
        {
            if ( Service != null )
                Service.Send(p);
        }

        /// <summary>
        /// Handles incoming <see cref="Packet"/>.
        /// </summary>
        /// <param name="p"><see cref="Packet"/> to handle.</param>
        internal void Handle( Packet p )
        {
            switch ( p.FirstOpcode )
            {
                case ServiceLayer.Identity:
                    {
                        switch ( p.SecondOpcode )
                        {
                            case ServiceLayer.InitializeRequest:
                                {
                                    Logger.WriteLine(Source.InnerNetwork, "Connected service requests connection initialization.");
                                    return;
                                }
                            case ServiceLayer.SetSettingsRequest:
                                {
                                    // set settings here
                                    return;
                                }
                        }

                        break;
                    }
                case WorldDataLayer.Identity:
                    {
                        switch ( p.SecondOpcode )
                        {
                            case WorldDataLayer.SetWorldActiveRequest:
                                {
                                    Realtime.RealtimeManager.WorldsInfo.SetActive(Service.ServiceID);
                                    Send(new SetWorldActiveResponse().ToPacket());
                                    Logger.WriteLine(Source.InnerNetwork, "World 0x{0:x2} allowed to accept user connections", Service.ServiceID);
                                    return;
                                }
                        }

                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            Logger.WriteLine(Source.InnerNetwork, "Unknown packet received from {0} service:{1}{2}", ServiceType.GameService, Environment.NewLine, p.ToString());
        }
    }
}