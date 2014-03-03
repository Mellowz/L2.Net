using L2.Net.Network;
using L2.Net.Structs.Services;

namespace L2.Net.NpcService.Network
{
    internal static class CacheServiceRequestsHandlers
    {
        internal static void Handle( Packet p )
        {
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
                                                CacheServiceConnection.Connection.SetRemoteService(data.RemoteServiceID, ( ServiceType )data.RemoteServiceType);
                                                Logger.WriteLine(Source.InnerNetwork, "Connected to {0} (0x{1})", CacheServiceConnection.Connection.ServiceType, CacheServiceConnection.Connection.ServiceID.ToString("x2"));

                                                // send set-settings request 
                                                //CacheServiceConnection.Send
                                                //    (
                                                //        new SetSettingsRequest().ToPacket
                                                //        (
                                                //            new LoginServiceSettings
                                                //                (
                                                //                    Service.LocalServiceUniqueId,
                                                //                    Service.AutoCreateUsers,
                                                //                    Service.DefaultAccessLevel
                                                //                )
                                                //        )
                                                //    );

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
                            case ServiceLayer.SetSettingsRequest:
                                {
                                    return; // login service not handles remote service settings request
                                }
                            case ServiceLayer.SetSettingsResponse:
                                {
                                    SetSettingsResponse response = new SetSettingsResponse(p);

                                    switch ( response.Response )
                                    {
                                        case SetSettingsResponse.Accepted:
                                            {
                                                Logger.WriteLine(Source.InnerNetwork, "Cache service accepted service settings.");
                                                //UserConnectionsListener.Enable(); // start listen incoming user connections
                                                return;
                                            }
                                        default:
                                            {
                                                Service.Terminate(new ServiceShutdownEventArgs("Cache service rejected settings setup."));
                                                return;
                                            }
                                    }
                                }
                            default:
                                {
                                    Logger.WriteLine("Unknown packet received on layer 0x{0}: {1}", ServiceLayer.Identity.ToString("x2"), p.ToString());
                                    return;
                                }
                        }
                    }
            }
        }
    }
}