namespace L2.Net.Structs.Services
{
    /// <summary>
    /// Set world as active response.
    /// </summary>
    public struct SetWorldActiveResponse
    {
        /// <summary>
        /// Packet representation opcodes. 
        /// </summary>
        public static readonly byte[] Opcodes = 
        { 
            WorldDataLayer.Identity,
            WorldDataLayer.SetWorldActiveResponse
        };

        /// <summary>
        /// Initializes new instance of <see cref="SetWorldActiveResponse"/> struct.
        /// </summary>
        /// <param name="p"><see cref="Packet"/> to initialize from.</param>
        public SetWorldActiveResponse( Packet p )
        {
        }

        /// <summary>
        /// Converts current struct to it's <see cref="Packet"/> equivalent.
        /// </summary>
        /// <returns><see cref="Packet"/> equivalent of current struct.</returns>
        public Packet ToPacket()
        {
            return new Packet(Opcodes);
        }
    }
}