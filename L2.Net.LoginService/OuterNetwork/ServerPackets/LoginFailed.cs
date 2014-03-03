using L2.Net.Network;
using L2.Net.Structs.Services;

namespace L2.Net.LoginService.OuterNetwork
{
    /// <summary>
    /// Login failed packet.
    /// </summary>
    internal static class LoginFailed
    {
        /// <summary>
        /// Packet opcode.
        /// </summary>
        private const byte Opcode = 0x01;

        /// <summary>
        /// Login failed server > client packet.
        /// </summary>
        /// <param name="response">Login failed reason.</param>
        /// <returns>Login failed <see cref="Packet"/>.</returns>
        internal static Packet ToPacket( UserAuthenticationResponseType response )
        {
            Packet p = new Packet(Opcode);
            p.WriteByte(( byte )response);
            return p;
        }
    }
}