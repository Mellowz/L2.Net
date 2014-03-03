namespace L2.Net.Structs.Services
{
    /// <summary>
    /// Request to cache server to un-cache all cached user data.
    /// </summary>
    public struct UnCacheUser
    {
        /// <summary>
        /// Packet representation opcodes.
        /// </summary>
        public static readonly byte[] Opcodes = 
        { 
            UserDataLayer.Identity, 
            UserDataLayer.UnCacheUser
        };

        /// <summary>
        /// User session unique id.
        /// </summary>
        public readonly int SessionID;

        /// <summary>
        /// Initializes new instance of <see cref="UnCacheUser"/> struct.
        /// </summary>
        /// <param name="sessionID">Session unique id.</param>
        public UnCacheUser( int sessionID )
        {
            SessionID = sessionID;
        }

        /// <summary>
        /// Initializes new instance of <see cref="UnCacheUser"/> struct.
        /// </summary>
        /// <param name="p"><see cref="Packet"/> to initialize from.</param>
        public UnCacheUser( Packet p )
        {
            SessionID = p.ReadInt();
        }

        /// <summary>
        /// Converts current struct to it's <see cref="Packet"/> equivalent.
        /// </summary>
        /// <returns><see cref="Packet"/> equivalent of current struct.</returns>
        public Packet ToPacket()
        {
            Packet p = new Packet(Opcodes);
            p.WriteInt(SessionID);
            return p;
        }
    }
}