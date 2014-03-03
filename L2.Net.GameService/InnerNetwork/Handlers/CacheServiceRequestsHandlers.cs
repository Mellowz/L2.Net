using L2.Net.GameService.Core;
using L2.Net.Network;
using L2.Net.Structs.Services;

namespace L2.Net.GameService.InnerNetwork
{
    internal static class CacheServiceRequestsHandlers
    {
        internal static void Handle( Packet p )
        {
            //Logger.WriteLine(p.ToString());

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
                                                Logger.WriteLine(Source.InnerNetwork, "Authorized on {0} (0x{1})", CacheServiceConnection.Connection.ServiceType, CacheServiceConnection.Connection.ServiceID.ToString("x2"));

                                                // send set-settings request if needed

                                                World.StartUp();

                                                return;
                                            }
                                        case InitializeResponse.Rejected:
                                            {
                                                Logger.WriteLine(Source.InnerNetwork, "Connection rejected by {0} (0x{1})", ( ServiceType )data.RemoteServiceType, data.RemoteServiceID);
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
                case WorldDataLayer.Identity:
                    {
                        switch ( p.SecondOpcode )
                        {
                            case WorldDataLayer.SetWorldActiveResponse:
                                Logger.WriteLine(Source.InnerNetwork, "World is ready for incoming user connections!");
                                break;
                        }
                        break;
                    }
            }
        }
    }
}