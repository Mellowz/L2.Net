namespace L2.Net.Structs.Services
{
    /// <summary>
    /// Worlds list response.
    /// </summary>
    public struct WorldsListResponse
    {
        /// <summary>
        /// Packet representation opcodes.
        /// </summary>
        public static readonly byte[] Opcodes = 
        { 
            UserDataLayer.Identity, 
            UserDataLayer.WorldsListResponse
        };

        /// <summary>
        /// <see cref="WorldsListRequest"/> unique id.
        /// </summary>
        public readonly long RequestID;

        /// <summary>
        /// Connected worlds data.
        /// </summary>
        public readonly WorldSummary[] Data;

        /// <summary>
        /// Initializes new instance of <see cref="WorldsListResponse"/> struct.
        /// </summary>
        /// <param name="requestID">Request unique id.</param>
        /// <param name="worlds">Connected worlds data.</param>
        public WorldsListResponse( long requestID, params WorldSummary[] worlds )
        {
            RequestID = requestID;
            Data = worlds;
        }

        /// <summary>
        /// Initializes new instance of <see cref="WorldsListResponse"/> struct.
        /// </summary>
        /// <param name="p"><see cref="Packet"/> to initialize from.</param>
        public WorldsListResponse( Packet p )
        {
            RequestID = p.ReadLong();
            byte count = p.ReadByte();
            Data = new WorldSummary[count];

            for ( int i = 0; i < count; i++ )
                Data[i] = new WorldSummary(ref p);
        }

        /// <summary>
        /// Converts current struct to it's <see cref="Packet"/> equivalent.
        /// </summary>
        /// <returns><see cref="Packet"/> equivalent of current struct.</returns>
        public Packet ToPacket()
        {
            Packet p = new Packet(Opcodes);

            p.WriteLong(RequestID);
            p.WriteByte(( byte )Data.Length);

            foreach ( WorldSummary ws in Data )
                WorldSummary.Write(ws, ref p);

            return p;
        }
    }
}