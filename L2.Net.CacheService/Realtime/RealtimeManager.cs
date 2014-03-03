using System.Collections.Generic;
using L2.Net.Network;
using L2.Net.Structs.Services;

namespace L2.Net.CacheService.Realtime
{
    /// <summary>
    /// Class, that manages real time operations.
    /// </summary>
    internal static class RealtimeManager
    {
        /// <summary>
        /// Startup operations.
        /// </summary>
        internal static void StartUp()
        {
            WorldsInfo.Cache();
        }

        /// <summary>
        /// Provides access to currently connected user sessions.
        /// </summary>
        internal static class ConnectedUsers
        {
            /// <summary>
            /// Collection of connected users <see cref="UserSession"/> objects.
            /// </summary>
            private static Dictionary<int, UserSession> m_ActiveSessions = new Dictionary<int, UserSession>();

            /// <summary>
            /// Collection of connected users by their logins / session ids.
            /// </summary>
            private static Dictionary<string, int> m_ActiveUsers = new Dictionary<string, int>();

            /// <summary>
            /// Indicates if session with provided session id exists in already connected sessions collection.
            /// </summary>
            /// <param name="sessionID"><see cref="UserSession"/> unique id.</param>
            /// <returns>True, if there is <see cref="UserSession"/> with provided unique id in collection, otherwise false.</returns>
            internal static bool Connected( int sessionID )
            {
                return m_ActiveSessions.ContainsKey(sessionID);
            }

            /// <summary>
            /// Indicates if user with provided login is already connected.
            /// </summary>
            /// <param name="login">User login.</param>
            /// <returns>True, if <see cref="UserSession"/> with provided <see cref="UserSession.AccountName"/> exists in collection, otherwise false.</returns>
            internal static bool Connected( string login )
            {
                return m_ActiveUsers.ContainsKey(login);
            }

            /// <summary>
            /// Registers provided <see cref="UserSession"/> in active sessions collection.
            /// </summary>
            /// <param name="session"><see cref="UserSession"/> to register.</param>
            internal static bool Register( UserSession session )
            {
                if ( session == UserSession.Null || Connected(session.ID) )
                    return false;

                m_ActiveSessions.Add(session.ID, session);
                m_ActiveUsers.Add(session.AccountName, session.ID);

                return true;
            }

            /// <summary>
            /// Updates user session data: sets current world id.
            /// </summary>
            /// <param name="sessionID"><see cref="UserSession"/> unique id.</param>
            /// <param name="worldID">Unique id of world, user just joined (or zero if disconnected).</param>
            internal static void UpdateCurrentWorld( int sessionID, byte worldID )
            {
                if ( m_ActiveSessions.ContainsKey(sessionID) )
                {
                    UserSession session = m_ActiveSessions[sessionID];
                    session.LastWorld = worldID;
                    m_ActiveSessions[sessionID] = session;
                }
            }

            /// <summary>
            /// Unregisters <see cref="UserSession"/> from connected users collection.
            /// </summary>
            /// <param name="sessionId"><see cref="UserSession"/> unique id.</param>
            internal static void Unregister( int sessionId )
            {
                if ( m_ActiveSessions.ContainsKey(sessionId) )
                {
                    m_ActiveUsers.Remove(m_ActiveSessions[sessionId].AccountName);
                    m_ActiveSessions.Remove(sessionId);
                }
            }

            /// <summary>
            /// Unregisters <see cref="UserSession"/> from connected users collection.
            /// </summary>
            /// <param name="login">User login.</param>
            internal static void Unregister( string login )
            {
                if ( m_ActiveUsers.ContainsKey(login) )
                {
                    m_ActiveSessions.Remove(m_ActiveUsers[login]);
                    m_ActiveUsers.Remove(login);
                }
            }

            /// <summary>
            /// Searches <see cref="UserSession"/> in active connections collection.
            /// </summary>
            /// <param name="sessionID"><see cref="UserSession"/> unique id.</param>
            /// <returns><see cref="UserSession"/> object if session was found, otherwise <see cref="UserSession.Null"/> value.</returns>
            internal static UserSession Find( int sessionID )
            {
                return m_ActiveSessions.ContainsKey(sessionID) ? m_ActiveSessions[sessionID] : UserSession.Null;
            }
        }

        /// <summary>
        /// Provides access to aviable / connected worlds data.
        /// </summary>
        internal static class WorldsInfo
        {
            /// <summary>
            /// Worlds info collection.
            /// </summary>
            private static SortedList<byte, WorldSummary> m_WorldsSummary = new SortedList<byte, WorldSummary>();

            /// <summary>
            /// Retrieves information about aviable worlds from database.
            /// </summary>
            internal static void Cache()
            {
                m_WorldsSummary.Clear();

                WorldSummary[] ws = DataProvider.DataBase.Worlds_Cache();

                foreach ( WorldSummary s in ws )
                {
                    if ( m_WorldsSummary.ContainsKey(s.ID) )
                        Logger.WriteLine(Source.Service, "Database records error - duplicated worlds unique ids found: {0}", s.ID);
                    else
                        m_WorldsSummary.Add(s.ID, s);
                }

                Logger.WriteLine(Source.Service, "Cached {0} worlds info", m_WorldsSummary.Count);
            }


            /// <summary>
            /// Gets <see cref="WorldSummary"/> array.
            /// </summary>
            /// <returns><see cref="WorldSummary"/> array.</returns>
            internal static WorldSummary[] Get()
            {
                WorldSummary[] current = new WorldSummary[m_WorldsSummary.Count];
                m_WorldsSummary.Values.CopyTo(current, 0);
                return current;
            }

            /// <summary>
            /// Sets world as online and ready to user connections.
            /// </summary>
            /// <param name="id">World unique id.</param>
            internal static void SetActive( byte id )
            {
                if ( m_WorldsSummary.ContainsKey(id) )
                {
                    WorldSummary ws = m_WorldsSummary[id];
                    ws.IsOnline = true;
                    m_WorldsSummary[id] = ws;
                }
            }

            /// <summary>
            /// Checks if current worlds info collection contains information about world with provided unique id.
            /// </summary>
            /// <param name="id">World unique id.</param>
            /// <returns>True, if world with provided unique id exists, otherwise false.</returns>
            internal static bool Contains( byte id )
            {
                return m_WorldsSummary.ContainsKey(id);
            }

            /// <summary>
            /// Returns true if world with provided unique id is online.
            /// </summary>
            /// <param name="id">World unique id.</param>
            /// <returns>True if world with provided unique id is online, otherwise false.</returns>
            internal static bool IsOnline( byte id )
            {
                return m_WorldsSummary.ContainsKey(id) ? m_WorldsSummary[id].IsOnline : false;
            }

            /// <summary>
            /// Returns true if world with provided unique id may accept next user.
            /// </summary>
            /// <param name="id">World unique id.</param>
            /// <returns>False, if world is not full, otherwise true.</returns>
            internal static bool IsFull( byte id )
            {
                return m_WorldsSummary.ContainsKey(id) ? m_WorldsSummary[id].UsersOnline >= m_WorldsSummary[id].UsersMax : true;
            }
        }
    }
}