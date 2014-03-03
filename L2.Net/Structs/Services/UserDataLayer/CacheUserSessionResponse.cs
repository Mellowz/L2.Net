namespace L2.Net.Structs.Services
{
    /// <summary>
    /// Cache user session data response.
    /// </summary>
    public struct CacheUserSessionResponse
    {
        /// <summary>
        /// Packet representation opcodes.
        /// </summary>
        public static readonly byte[] Opcodes = 
        { 
            UserDataLayer.Identity, 
            UserDataLayer.CacheUserSessionResponse
        };

        /// <summary>
        /// Session registration failed.
        /// </summary>
        public const byte Failed = 0x00;

        /// <summary>
        /// Session successfully registered.
        /// </summary>
        public const byte Accepted = 0x01;

        /// <summary>
        /// <see cref="CacheUserSessionResponse"/> unique id.
        /// </summary>
        public readonly long RequestID;

        /// <summary>
        /// Cache service response.
        /// </summary>
        public readonly byte Response;

        /// <summary>
        /// Initializes new instance of <see cref="CacheUserSessionResponse"/> struct.
        /// </summary>
        /// <param name="requestID"><see cref="CacheUserSessionResponse"/> unique id.</param>
        /// <param name="response">Cache service response.</param>
        public CacheUserSessionResponse( long requestID, byte response )
        {
            RequestID = requestID;
            Response = response;
        }

        /// <summary>
        /// Initializes new instance of  <see cref="CacheUserSessionResponse"/> struct.
        /// </summary>
        /// <param name="p"><see cref="Packet"/> to initialize from.</param>
        public CacheUserSessionResponse( Packet p )
        {
            RequestID = p.ReadLong();
            Response = p.ReadByte();
        }

        /// <summary>
        /// Converts current struct to it's <see cref="Packet"/> equivalent.
        /// </summary>
        /// <returns><see cref="Packet"/> equivalent of current struct.</returns>
        public Packet ToPacket()
        {
            Packet p = new Packet(Opcodes);
            p.WriteLong(RequestID);
            p.WriteByte(Response);
            return p;
        }
    }
}