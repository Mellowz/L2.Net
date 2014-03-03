using System;
using L2.Net.LoginService.OuterNetwork;
using L2.Net.LoginService.Properties;
using L2.Net.Network;
using L2.Net.Structs.Services;

namespace L2.Net.LoginService.InnerNetwork
{
    /// <summary>
    /// Handles cache service requests.
    /// </summary>
    internal static class CacheServiceRequestsHandlers
    {
        /// <summary>
        /// Handles incoming packet.
        /// </summary>
        /// <param name="p">Received <see cref="Packet"/> object.</param>
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
                                                Logger.WriteLine(Source.InnerNetwork, "Authorized on {0} (0x{1})", CacheServiceConnection.Connection.ServiceType, CacheServiceConnection.Connection.ServiceID.ToString("x2"));

                                                // send set-settings request 
                                                CacheServiceConnection.Send
                                                    (
                                                        new SetSettingsRequest().ToPacket
                                                        (
                                                            new LoginServiceSettings
                                                                (
                                                                    Settings.Default.ServiceUniqueID,
                                                                    Settings.Default.LoginServiceAutoCreateUsers,
                                                                    Settings.Default.LoginServiceDefaultAccessLevel
                                                                )
                                                        )
                                                    );

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
                case UserDataLayer.Identity:
                    {
                        switch ( p.SecondOpcode )
                        {
                            case UserDataLayer.UserAuthenticationResponse:
                                {
                                    UserAuthenticationResponse response = new UserAuthenticationResponse(p);
                                    QueuedRequest request = QueuedRequestsPool.Dequeue(response.RequestID);

                                    if ( QueuedRequest.IsValid(request) )
                                    {
                                        switch ( response.Response )
                                        {
                                            case UserAuthenticationResponseType.UserAccepted: // accepted or created by cache
                                                {
                                                    request.UserConnection.Session.AccountID = response.UserID;
                                                    request.UserConnection.Session.LastWorld = response.LastWorldID;

                                                    if ( response.AccessLevel < Settings.Default.LoginServiceAllowedAccessLevel )
                                                    {
                                                        request.Send(LoginFailed.ToPacket(UserAuthenticationResponseType.AccessFailed));
                                                        UserConnectionsListener.CloseActiveConnection(request.UserConnection);
                                                        return;
                                                    }

                                                    long requestId = long.MinValue;

                                                    if ( QueuedRequestsPool.Enqueue(request.UserConnection, ref requestId) )
                                                    {
                                                        CacheServiceConnection.Send
                                                            (
                                                                new CacheUserSessionRequest
                                                                    (
                                                                        requestId,
                                                                        request.UserConnection.Session
                                                                    ).ToPacket()
                                                            );
                                                    }
                                                    else
                                                    {
                                                        Logger.WriteLine(Source.InnerNetwork, "Failed to send CacheUserSessionRequest to cache service, request was not enqueued by QueuedRequestsPool ?...");
                                                        UserConnectionsListener.CloseActiveConnection(request.UserConnection);
                                                    }

                                                    return;
                                                }
                                            default:
                                                {
                                                    request.Send(LoginFailed.ToPacket(response.Response));
                                                    UserConnectionsListener.CloseActiveConnection(request.UserConnection);
                                                    return;
                                                }
                                        }
                                    }

                                    break;
                                }
                            case UserDataLayer.CacheUserSessionResponse:
                                {
                                    CacheUserSessionResponse response = new CacheUserSessionResponse(p);
                                    QueuedRequest request = QueuedRequestsPool.Dequeue(response.RequestID);

                                    if ( QueuedRequest.IsValid(request) )
                                    {
                                        switch ( response.Response )
                                        {
                                            case CacheUserSessionResponse.Failed:
                                                {
                                                    Logger.WriteLine("Failed to cache user session data on cache server side");
                                                    request.Send(LoginFailed.ToPacket(UserAuthenticationResponseType.SystemError));
                                                    UserConnectionsListener.CloseActiveConnection(request.UserConnection);
                                                    return;
                                                }
                                            case CacheUserSessionResponse.Accepted:
                                                {
                                                    request.Send(LoginOk.ToPacket(request.UserConnection.Session));
                                                    return;
                                                }
                                        }
                                    }
                                    else
                                    {
                                        Logger.WriteLine(Source.OuterNetwork, "Failed to send ServerPackets.WorldsList to client, request was not dequeued by QueuedRequestsPool ?...");
                                        UserConnectionsListener.CloseActiveConnection(request.UserConnection);
                                    }

                                    return;
                                }
                            case UserDataLayer.WorldsListResponse:
                                {
                                    WorldsListResponse response = new WorldsListResponse(p);
                                    QueuedRequest request = QueuedRequestsPool.Dequeue(response.RequestID);

                                    if ( QueuedRequest.IsValid(request) )
                                        request.Send(ServerList.ToPacket(request.UserConnection.Session.LastWorld, response.Data));
                                    else
                                    {
                                        Logger.WriteLine(Source.OuterNetwork, "Failed to send ServerPackets.WorldsList to client, request was not dequeued by QueuedRequestsPool ?...");
                                        UserConnectionsListener.CloseActiveConnection(request.UserConnection);
                                    }

                                    return;
                                }
                            case UserDataLayer.JoinWorldResponse:
                                {
                                    JoinWorldResponse response = new JoinWorldResponse(p);
                                    QueuedRequest request = QueuedRequestsPool.Dequeue(response.RequestID);

                                    if ( QueuedRequest.IsValid(request) )
                                    {
                                        switch ( response.Result )
                                        {
                                            case JoinWorldRequestResult.Accepted:
                                                {
                                                    request.UserConnection.Send(PlayAccepted.ToPacket(request.UserConnection.Session));
                                                    break;
                                                }
                                            default:
                                                {
                                                    request.UserConnection.Send(PlayFail.ToPacket(response.Result));
                                                    break;
                                                }
                                        }
                                    }

                                    UserConnectionsListener.CloseConnectionWithoutLogout(request.UserConnection);

                                    return;
                                }
                        }

                        break;
                    }
            }

            Logger.WriteLine("Unknown packet received from {0} service on layer 0x{1}:{2}{3}", ServiceType.LoginService, p.FirstOpcode.ToString("x2"), Environment.NewLine, p.ToString());
        }
    }
}