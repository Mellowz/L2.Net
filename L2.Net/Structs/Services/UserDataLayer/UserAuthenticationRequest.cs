namespace L2.Net.Structs.Services
{
    /// <summary>
    /// User authentication request.
    /// </summary>
    public struct UserAuthenticationRequest
    {
        /// <summary>
        /// Packet representation opcodes.
        /// </summary>
        public static readonly byte[] Opcodes = 
        { 
            UserDataLayer.Identity, 
            UserDataLayer.AuthenticateUser 
        };

        /// <summary>
        /// <see cref="UserAuthenticationRequest"/> request unique id.
        /// </summary>
        public readonly long RequestID;

        /// <summary>
        /// User login.
        /// </summary>
        public readonly string Login;

        /// <summary>
        /// User password.
        /// </summary>
        public readonly string Password;

        /// <summary>
        /// <see cref="L2.Net.UserSession"/> unique id.
        /// </summary>
        public readonly int SessionID;

        /// <summary>
        /// Initializes new instance of <see cref="UserAuthenticationRequest"/> struct.
        /// </summary>
        /// <param name="requestID"><see cref="UserAuthenticationRequest"/> request unique id.</param>
        /// <param name="login">User login.</param>
        /// <param name="password">User password.</param>
        /// <param name="sessionID">User session unique id.</param>
        public UserAuthenticationRequest( long requestID, string login, string password, int sessionID )
        {
            RequestID = requestID;
            Login = login;
            Password = password;
            SessionID = sessionID;
        }

        /// <summary>
        /// Initializes new instance of <see cref="UserAuthenticationRequest"/> struct.
        /// </summary>
        /// <param name="p"><see cref="Packet"/> to initialize from.</param>
        public UserAuthenticationRequest( Packet p )
        {
            RequestID = p.ReadLong();
            Login = p.ReadString();
            Password = p.ReadString();
            SessionID = p.ReadInt();
        }

        /// <summary>
        /// Converts current struct to it's <see cref="Packet"/> equivalent.
        /// </summary>
        /// <returns><see cref="Packet"/> equivalent of current struct.</returns>
        public Packet ToPacket()
        {
            Packet p = new Packet(Opcodes);

            p.WriteLong(RequestID);

            p.WriteString
                (
                    Login,
                    Password
                );

            p.WriteInt(SessionID);

            return p;
        }
    }
}