using System;
using L2.Net.CacheService.Realtime;
using L2.Net.Network;
using L2.Net.Structs.Services;

namespace L2.Net.CacheService.Network.Handlers
{
    /// <summary>
    /// Login service requests handler.
    /// </summary>
    internal sealed class LoginServiceRequestsHandlers
    {
        /// <summary>
        /// Inner network client for incoming packets handling.
        /// </summary>
        internal InnerNetworkClient Service;

        /// <summary>
        /// Initializes new instance of <see cref="LoginServiceRequestsHandlers"/> class.
        /// </summary>
        /// <param name="client">Referenced <see cref="InnerNetworkClient"/> object.</param>
        internal LoginServiceRequestsHandlers( ref InnerNetworkClient client )
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
                                    LoginServiceSettings settings = ( LoginServiceSettings )SetSettingsRequest.FromPacket(p, ServiceType.LoginService);
                                    ConnectionsManager.SetServiceSettings(settings);
                                    return;
                                }
                        }

                        break;
                    }
                case UserDataLayer.Identity:
                    {
                        switch ( p.SecondOpcode )
                        {
                            case UserDataLayer.AuthenticateUser:
                                {
                                    UserAuthenticationRequest request = new UserAuthenticationRequest(p);

                                    if ( RealtimeManager.ConnectedUsers.Connected(request.SessionID) )
                                    {
                                        Send(new UserAuthenticationResponse(request.RequestID, UserAuthenticationResponseType.AccessFailed).ToPacket());
                                        return;
                                    }

                                    if ( RealtimeManager.ConnectedUsers.Connected(request.Login) )
                                    {
                                        Send(new UserAuthenticationResponse(request.RequestID, UserAuthenticationResponseType.AccountInUse).ToPacket());
                                        return;
                                    }

                                    Send(DataProvider.DataBase.User_Auth(request, ( LoginServiceSettings )Service.RemoteServiceSettings).ToPacket());

                                    return;
                                }
                            case UserDataLayer.CacheUserSessionRequest:
                                {
                                    CacheUserSessionRequest request = new CacheUserSessionRequest(p);

                                    if ( RealtimeManager.ConnectedUsers.Connected(request.Session.ID) || RealtimeManager.ConnectedUsers.Connected(request.Session.AccountName) )
                                    {
                                        Send(new CacheUserSessionResponse(request.RequestID, CacheUserSessionResponse.Failed).ToPacket());
                                        return;
                                    }

                                    RealtimeManager.ConnectedUsers.Register(request.Session);

                                    Send(new CacheUserSessionResponse(request.RequestID, CacheUserSessionResponse.Accepted).ToPacket());

                                    return;
                                }
                            case UserDataLayer.WorldsListRequest:
                                {
                                    WorldsListRequest request = new WorldsListRequest(p);
                                    Send(new WorldsListResponse(request.RequestID, RealtimeManager.WorldsInfo.Get()).ToPacket());
                                    return;
                                }
                            case UserDataLayer.UnCacheUser:
                                {
                                    UnCacheUser request = new UnCacheUser(p);

                                    // update user login / logout / used_time values in database.

                                    UserSession session = RealtimeManager.ConnectedUsers.Find(request.SessionID);

                                    if ( session != UserSession.Null )
                                    {
                                        DataProvider.DataBase.User_Logout(session.AccountID, session.StartTime, session.IPAddress, session.LastWorld);
                                        RealtimeManager.ConnectedUsers.Unregister(request.SessionID);
                                    }

                                    return;
                                }
                            case UserDataLayer.JoinWorldRequest:
                                {
                                    // check access level

                                    JoinWorldRequest request = new JoinWorldRequest(p);

                                    if ( !RealtimeManager.ConnectedUsers.Connected(request.SessionID) )
                                    {
                                        Send(new JoinWorldResponse(request.RequestID, JoinWorldRequestResult.AccessFailed).ToPacket());
                                        return;
                                    }

                                    if ( !RealtimeManager.WorldsInfo.Contains(request.WorldID) || !RealtimeManager.WorldsInfo.IsOnline(request.WorldID) )
                                    {
                                        Send(new JoinWorldResponse(request.RequestID, JoinWorldRequestResult.SystemError).ToPacket());
                                        return;
                                    }

                                    if ( RealtimeManager.WorldsInfo.IsFull(request.WorldID) )
                                    {
                                        Send(new JoinWorldResponse(request.RequestID, JoinWorldRequestResult.TooManyPlayers).ToPacket());
                                        return;
                                    }

                                    Send(new JoinWorldResponse(request.RequestID, JoinWorldRequestResult.Accepted).ToPacket());
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

            Logger.WriteLine(Source.InnerNetwork, "Unknown packet received from {0} service:{1}{2}", ServiceType.LoginService, Environment.NewLine, p.ToString());
        }
    }
}