namespace L2.Net.Structs.Services
{
    /// <summary>
    /// Request join world packet.
    /// </summary>
    public struct JoinWorldRequest
    {
        /// <summary>
        /// Packet representation opcodes.
        /// </summary>
        public static readonly byte[] Opcodes = 
        { 
            UserDataLayer.Identity, 
            UserDataLayer.JoinWorldRequest
        };

        /// <summary>
        /// <see cref="JoinWorldRequest"/> request unique id.
        /// </summary>
        public readonly long RequestID;

        /// <summary>
        /// <see cref="L2.Net.UserSession"/> unique id.
        /// </summary>
        public readonly int SessionID;

        /// <summary>
        /// World id, user selected to join. 
        /// </summary>
        public readonly byte WorldID;

        /// <summary>
        /// Initializes new instance of <see cref="JoinWorldRequest"/> struct.
        /// </summary>
        /// <param name="requestID"><see cref="JoinWorldRequest"/> request unique id.</param>
        /// <param name="sessionID">User session unique id.</param>
        /// <param name="worldID">Selected world id.</param>
        public JoinWorldRequest( long requestID, int sessionID, byte worldID )
        {
            RequestID = requestID;
            SessionID = sessionID;
            WorldID = worldID;
        }

        /// <summary>
        /// Initializes new instance of <see cref="JoinWorldRequest"/> struct.
        /// </summary>
        /// <param name="p"><see cref="Packet"/> to initialize from.</param>
        public JoinWorldRequest( Packet p )
        {
            RequestID = p.ReadLong();
            SessionID = p.ReadInt();
            WorldID = p.ReadByte();
        }

        /// <summary>
        /// Converts current struct to it's <see cref="Packet"/> equivalent.
        /// </summary>
        /// <returns><see cref="Packet"/> equivalent of current struct.</returns>
        public Packet ToPacket()
        {
            Packet p = new Packet(Opcodes);
            p.WriteLong(RequestID);
            p.WriteInt(SessionID);
            p.WriteByte(WorldID);
            return p;
        }
    }
}