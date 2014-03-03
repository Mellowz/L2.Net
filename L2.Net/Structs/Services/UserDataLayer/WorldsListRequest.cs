namespace L2.Net.Structs.Services
{
    /// <summary>
    /// Worlds list request.
    /// </summary>
    public struct WorldsListRequest
    {
        /// <summary>
        /// Packet representation opcodes.
        /// </summary>
        public static readonly byte[] Opcodes = 
        { 
            UserDataLayer.Identity, 
            UserDataLayer.WorldsListRequest
        };

        /// <summary>
        /// <see cref="WorldsListRequest"/> unique id.
        /// </summary>
        public readonly long RequestID;

        /// <summary>
        /// Initializes new instance of <see cref="WorldsListRequest"/> struct.
        /// </summary>
        /// <param name="requestID"><see cref="WorldsListRequest"/> unique id.</param>
        public WorldsListRequest( long requestID )
        {
            RequestID = requestID;
        }

        /// <summary>
        /// Initializes new instance of <see cref="WorldsListRequest"/> struct.
        /// </summary>
        /// <param name="p"><see cref="Packet"/> to initialize from.</param>
        public WorldsListRequest( Packet p )
        {
            RequestID = p.ReadLong();
        }

        /// <summary>
        /// Converts current struct to it's <see cref="Packet"/> equivalent.
        /// </summary>
        /// <returns><see cref="Packet"/> equivalent of current struct.</returns>
        public Packet ToPacket()
        {
            Packet p = new Packet(Opcodes);
            p.WriteLong(RequestID);
            return p;
        }
    }
}