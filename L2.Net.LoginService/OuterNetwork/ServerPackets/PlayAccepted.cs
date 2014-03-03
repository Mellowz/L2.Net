using L2.Net.Network;

namespace L2.Net.LoginService.OuterNetwork
{
    /// <summary>
    /// Play accepted packet.
    /// </summary>
    internal static class PlayAccepted
    {
        /// <summary>
        /// Packet opcode.
        /// </summary>
        private const byte Opcode = 0x07;

        /// <summary>
        /// Returns play accepted server > client packet.
        /// </summary>
        /// <param name="session"><see cref="UserSession"/> object.</param>
        /// <returns>Play accepted <see cref="Packet"/>.</returns>
        internal static Packet ToPacket( UserSession session )
        {
            Packet p = new Packet(Opcode);
            p.WriteInt(session.Play1, session.Play2);
            return p;
        }
    }
}