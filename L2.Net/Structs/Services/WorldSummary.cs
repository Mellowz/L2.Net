namespace L2.Net.Structs.Services
{
    /// <summary>
    /// Represents world summary struct. 
    /// </summary>
    public struct WorldSummary
    {
        /// <summary>
        /// World unique id.
        /// </summary>
        public byte ID;

        /// <summary>
        /// World ip-address in bytes.
        /// </summary>
        public byte[] Address;

        /// <summary>
        /// Port, that world use.
        /// </summary>
        public int Port;

        /// <summary>
        /// Age limit.
        /// </summary>
        public byte AgeLimit;

        /// <summary>
        /// Indicates if world is PvP server.
        /// </summary>
        public bool IsPvP;

        /// <summary>
        /// Maximum of connected users count.
        /// </summary>
        public short UsersMax;

        /// <summary>
        /// Currently connected users count.
        /// </summary>
        public short UsersOnline;

        /// <summary>
        /// Indicates if is needed to show brackets next to world name.
        /// </summary>
        public bool ShowBrackets;

        /// <summary>
        /// Indicates if world is test server.
        /// </summary>
        public bool IsTestServer;

        /// <summary>
        /// Indicates if is needed to show clock next to world name.
        /// </summary>
        public bool ShowClock;

        /// <summary>
        /// Indicates if world is online now.
        /// </summary>
        public bool IsOnline;

        /// <summary>
        /// Minimum access level, needed to connect to current world.
        /// </summary>
        public byte AccessLevel;

        /// <summary>
        /// Initializes new instance of <see cref="WorldSummary"/> struct.
        /// </summary>
        /// <param name="id">World id.</param>
        /// <param name="address">Ip-address as <see cref="byte"/> array.</param>
        /// <param name="port">Port.</param>
        /// <param name="ageLimit">Age limit.</param>
        /// <param name="isPvP">True, if world is PvP server.</param>
        /// <param name="usersMax">Max connected users count.</param>
        /// <param name="usersOnline">Currently connected users count.</param>
        /// <param name="showBrackets">If true, show brackets before world name.</param>
        /// <param name="isTestServer">True, if world is test server.</param>
        /// <param name="showClock">If true, show clock near world name.</param>
        ///<param name="isOnline">True, if world is online, otherwise false.</param>
        /// <param name="accessLevel">Minimum access level, required to connect to current world.</param>
        public WorldSummary( byte id, byte[] address, int port, byte ageLimit, bool isPvP, short usersMax, short usersOnline, bool showBrackets, bool isTestServer, bool showClock, bool isOnline, byte accessLevel )
        {
            ID = id;
            Address = address;
            Port = port;
            AgeLimit = ageLimit;
            IsPvP = isPvP;
            UsersMax = usersMax;
            UsersOnline = usersOnline;
            ShowBrackets = showBrackets;
            IsTestServer = isTestServer;
            ShowClock = showClock;
            IsOnline = isOnline;
            AccessLevel = accessLevel;
        }

        /// <summary>
        /// Initializes new instance of <see cref="WorldSummary"/> struct.
        /// </summary>
        /// <param name="p"><see cref="Packet"/> to read self data from.</param>
        public WorldSummary( ref Packet p )
        {
            ID = p.ReadByte();
            Address = p.ReadBytesArray(4);
            Port = p.ReadInt();
            AgeLimit = p.ReadByte();
            IsPvP = p.InternalReadBool();
            UsersMax = p.ReadShort();
            UsersOnline = p.ReadShort();
            ShowBrackets = p.InternalReadBool();
            IsTestServer = p.InternalReadBool();
            ShowClock = p.InternalReadBool();
            IsOnline = p.InternalReadBool();
            AccessLevel = p.ReadByte();
        }

        /// <summary>
        /// Appends world data to referenced <see cref="Packet"/> struct.
        /// </summary>
        /// <param name="ws"><see cref="WorldSummary"/> to write.</param>
        /// <param name="p">Referenced <see cref="Packet"/> struct.</param>
        public static void Write( WorldSummary ws, ref Packet p )
        {
            p.WriteByte(ws.ID);
            p.WriteBytesArray(ws.Address);
            p.WriteInt(ws.Port);
            p.WriteByte(ws.AgeLimit);
            p.InternalWriteBool(ws.IsPvP);
            p.WriteShort(ws.UsersMax);
            p.WriteShort(ws.UsersOnline);
            p.InternalWriteBool(ws.ShowBrackets);
            p.InternalWriteBool(ws.IsTestServer);
            p.InternalWriteBool(ws.ShowClock);
            p.InternalWriteBool(ws.IsOnline);
            p.WriteByte(ws.AccessLevel);
        }
    }
}