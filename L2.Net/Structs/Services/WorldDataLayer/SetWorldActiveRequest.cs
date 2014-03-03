namespace L2.Net.Structs.Services
{
    /// <summary>
    /// Notification from game server that it's ready to accept player connections.
    /// </summary>
    public struct SetWorldActiveRequest
    {
        /// <summary>
        /// Packet representation opcodes. 
        /// </summary>
        public static readonly byte[] Opcodes = 
        { 
            WorldDataLayer.Identity,
            WorldDataLayer.SetWorldActiveRequest 
        };

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