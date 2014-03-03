using L2.Net.Network;

namespace L2.Net.LoginService.OuterNetwork
{
    /// <summary>
    /// Initialize connection packet.
    /// </summary>
    internal static class InitializeConnection
    {
        /// <summary>
        /// Packet opcode.
        /// </summary>
        private const byte Opcode = 0x00;

        /// <summary>
        /// Returns connection initialization server > client <see cref="Packet"/>.
        /// </summary>
        /// <param name="session"><see cref="UserSession"/> object.</param>
        /// <returns>Connection initialization <see cref="Packet"/>.</returns>
        internal static Packet ToPacket( UserSession session )
        {
            Packet p = new Packet(Opcode);
            p.WriteInt(session.ID, 0x00);
            p.WriteBytesArray(UserConnectionsListener.ScrambledKeysPair.ScrambledModulus);
            p.WriteInt(0x00, 0x00, 0x00, 0x00);
            p.WriteBytesArray(session.BlowfishKey);
            p.WriteByte(0x00);
            return p;
        }
    }
}