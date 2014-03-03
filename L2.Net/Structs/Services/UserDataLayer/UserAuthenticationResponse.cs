namespace L2.Net.Structs.Services
{
    /// <summary>
    /// User authentication response types.
    /// </summary>
    public enum UserAuthenticationResponseType : byte
    {
        /// <summary>
        /// System error flag.
        /// </summary>
        SystemError = 0x01,
        /// <summary>
        /// Password wrong flag.
        /// </summary>
        PasswordWrong = 0x02,
        /// <summary>
        /// User or password wrong flag.
        /// </summary>
        UserOrPasswordWrong = 0x03,
        /// <summary>
        /// Access failed flag.
        /// </summary>
        AccessFailed = 0x04,
        /// <summary>
        /// Account in use flag.
        /// </summary>
        AccountInUse = 0x07,
        /// <summary>
        /// Server is overloaded flag.
        /// </summary>
        ServerOverloaded = 0x0f,
        /// <summary>
        /// Server is under maintenance flag.
        /// </summary>
        ServerMaintenance = 0x10,
        /// <summary>
        /// Temporary password expired flag.
        /// </summary>
        TemporaryPasswordExpired = 0x11,
        /// <summary>
        /// Dual boxing flag.
        /// </summary>
        DualBox = 0x23,
        /// <summary>
        /// Inner - user accepted flag.
        /// </summary>
        UserAccepted = 0xfd
    }

    /// <summary>
    /// User authentication response.
    /// </summary>
    public struct UserAuthenticationResponse
    {
        /// <summary>
        /// Packet representation opcodes.
        /// </summary>
        public static readonly byte[] Opcodes = 
        {
            UserDataLayer.Identity,
            UserDataLayer.UserAuthenticationResponse
        };

        /// <summary>
        /// Request unique id.
        /// </summary>
        public readonly long RequestID;

        /// <summary>
        /// Cache service response.
        /// </summary>
        public UserAuthenticationResponseType Response;

        /// <summary>
        /// User unique id.
        /// </summary>
        public int UserID;

        /// <summary>
        /// Last world user was played on.
        /// </summary>
        public byte LastWorldID;

        /// <summary>
        /// User's access level.
        /// </summary>
        public byte AccessLevel;

        /// <summary>
        /// Initializes new instance of <see cref="UserAuthenticationResponse"/> struct.
        /// </summary>
        /// <param name="requestID">Request unique id.</param>
        /// <param name="response">Cache service response.</param>
        public UserAuthenticationResponse( long requestID, UserAuthenticationResponseType response )
        {
            RequestID = requestID;
            Response = response;
            UserID = -1;
            LastWorldID = 1;
            AccessLevel = 0;
        }

        /// <summary>
        /// Initializes new instance of <see cref="UserAuthenticationResponse"/> struct.
        /// </summary>
        /// <param name="requestID">Request unique id.</param>
        /// <param name="response">Cache service response.</param>
        /// <param name="userID">User unique id.</param>
        /// <param name="lastWorldID">Last world id, player was played in.</param>
        /// <param name="accessLevel">User access level.</param>
        public UserAuthenticationResponse( long requestID, UserAuthenticationResponseType response, int userID, byte lastWorldID, byte accessLevel )
        {
            RequestID = requestID;
            Response = response;
            UserID = userID;
            LastWorldID = lastWorldID;
            AccessLevel = accessLevel;
        }

        /// <summary>
        /// Initializes new instance of <see cref="UserAuthenticationResponse"/> struct.
        /// </summary>
        /// <param name="p"><see cref="Packet"/> to initialize from.</param>
        public UserAuthenticationResponse( Packet p )
        {
            RequestID = p.ReadLong();
            Response = ( UserAuthenticationResponseType )p.ReadByte();

            switch ( Response )
            {
                case UserAuthenticationResponseType.UserAccepted:
                    {
                        UserID = p.ReadInt();
                        LastWorldID = p.ReadByte();
                        AccessLevel = p.ReadByte();
                        return;
                    }
                default:
                    {
                        UserID = -1;
                        LastWorldID = 1;
                        AccessLevel = 0;
                        return;
                    }
            }
        }

        /// <summary>
        /// Converts current struct to it's <see cref="Packet"/> equivalent.
        /// </summary>
        /// <returns><see cref="Packet"/> equivalent of current struct.</returns>
        public Packet ToPacket()
        {
            Packet p = new Packet(Opcodes);

            p.WriteLong(RequestID);
            p.WriteByte(( byte )Response);

            switch ( Response )
            {
                case UserAuthenticationResponseType.UserAccepted:
                    {
                        p.WriteInt(UserID);
                        p.WriteByte
                            (
                                LastWorldID,
                                AccessLevel
                            );
                        break;
                    }
                default:
                    break;
            }

            return p;
        }
    }
}