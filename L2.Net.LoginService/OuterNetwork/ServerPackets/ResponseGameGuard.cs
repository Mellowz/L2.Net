using L2.Net.Network;

namespace L2.Net.LoginService.OuterNetwork
{
    /// <summary>
    /// Game guard auth response.
    /// </summary>
    internal static class ResponseAuthGameGuard
    {
        /// <summary>
        /// Packet opcode.
        /// </summary>
        private const byte Opcode = 0x0b;

        /// <summary>
        /// Static packet implementation.
        /// </summary>
        internal static Packet Static = new Packet(Opcode);
    }
}
