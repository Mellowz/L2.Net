using L2.Net.Network;
using L2.Net.Structs.Services;

namespace L2.Net.LoginService.OuterNetwork
{
    /// <summary>
    /// Play failed packet.
    /// </summary>
    internal static class PlayFail
    {
        /// <summary>
        /// Packet opcode.
        /// </summary>
        private const byte Opcode = 0x06;

        /// <summary>
        /// Play failed server > client packet.
        /// </summary>
        /// <param name="reason">Play failed reason.</param>
        /// <returns>Play failed <see cref="Packet"/>.</returns>
        internal static Packet ToPacket( JoinWorldRequestResult reason )
        {
            Packet p = new Packet(Opcode);
            p.WriteByte(( byte )reason);
            return p;
        }
    }
}