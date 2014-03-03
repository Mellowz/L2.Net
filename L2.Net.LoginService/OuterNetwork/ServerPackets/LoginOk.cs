using L2.Net.Network;

namespace L2.Net.LoginService.OuterNetwork
{
    /// <summary>
    /// Login accepted packet.
    /// </summary>
    internal static class LoginOk
    {
        /// <summary>
        /// Packet opcode.
        /// </summary>
        private const byte Opcode = 0x03;

        /// <summary>
        /// Login accepted server > client packet.
        /// </summary>
        /// <param name="session"><see cref="UserSession"/> object.</param>
        /// <returns>Login accepted <see cref="Packet"/>.</returns>
        internal static Packet ToPacket( UserSession session )
        {
            Packet p = new Packet(Opcode);
            p.WriteInt(session.Login1, session.Login2);
            return p;
        }
    }
}