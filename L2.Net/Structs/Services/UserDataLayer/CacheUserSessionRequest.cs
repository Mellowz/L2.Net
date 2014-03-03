namespace L2.Net.Structs.Services
{
    /// <summary>
    /// Cache user session data request.
    /// </summary>
    public struct CacheUserSessionRequest
    {
        /// <summary>
        /// Packet representation opcodes.
        /// </summary>
        public static readonly byte[] Opcodes = 
        { 
            UserDataLayer.Identity, 
            UserDataLayer.CacheUserSessionRequest
        };

        /// <summary>
        /// <see cref="CacheUserSessionRequest"/> unique id.
        /// </summary>
        public readonly long RequestID;

        /// <summary>
        /// User session object.
        /// </summary>
        public readonly UserSession Session;

        /// <summary>
        /// Initializes new instance of <see cref="CacheUserSessionRequest"/> struct.
        /// </summary>
        /// <param name="requestID"><see cref="CacheUserSessionRequest"/> unique id.</param>
        /// <param name="session"><see cref="UserSession"/> object to cache.</param>
        public CacheUserSessionRequest( long requestID, UserSession session )
        {
            RequestID = requestID;
            Session = session;
        }

        /// <summary>
        /// Initializes new instance of <see cref="CacheUserSessionRequest"/> struct.
        /// </summary>
        /// <param name="p"><see cref="Packet"/> to initialize from.</param>
        public CacheUserSessionRequest( Packet p )
        {
            RequestID = p.ReadLong();

            UserSession session = new UserSession();
            session.AccountName = p.ReadString();
            session.IPAddress = p.ReadString();
            session.ID = p.ReadInt();
            session.AccountID = p.ReadInt();
            session.Login1 = p.ReadInt();
            session.Login2 = p.ReadInt();
            session.Play1 = p.ReadInt();
            session.Play2 = p.ReadInt();
            session.StartTime = p.InternalReadDateTime();

            Session = session;
        }

        /// <summary>
        /// Converts current struct to it's <see cref="Packet"/> equivalent.
        /// </summary>
        /// <returns><see cref="Packet"/> equivalent of current struct.</returns>
        public Packet ToPacket()
        {
            Packet p = new Packet(Opcodes);

            p.WriteLong(RequestID);

            p.WriteString(Session.AccountName);
            p.WriteString(Session.IPAddress);

            p.WriteInt
                (
                    Session.ID,
                    Session.AccountID,
                    Session.Login1,
                    Session.Login2,
                    Session.Play1,
                    Session.Play2
                );

            p.InternalWriteDateTime(Session.StartTime);

            return p;
        }
    }
}