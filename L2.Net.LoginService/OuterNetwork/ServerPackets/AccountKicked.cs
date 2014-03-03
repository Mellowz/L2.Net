using L2.Net.Network;

namespace L2.Net.LoginService.OuterNetwork
{
    /// <summary>
    /// Account kicked reason.
    /// </summary>
    internal enum AccountKeckedReason : byte
    {
        PaypmentAgreement = 0x01,
        GenericViolation = 0x08,
        SevenDaysSuspended = 0x10,
        PermanentlyBanned = 0x20
    }

    /// <summary>
    /// Account kicked packet.
    /// </summary>
    internal static class AccountKicked
    {
        /// <summary>
        /// Packet opcode.
        /// </summary>
        private const byte Opcode = 0x02;

        /// <summary>
        /// Returns account kicked packet.
        /// </summary>
        /// <param name="reason">Account kicked reason.</param>
        /// <returns>Account kicked <see cref="Packet"/>.</returns>
        internal static Packet ToPacket( AccountKeckedReason reason )
        {
            Packet p = new Packet(Opcode);
            p.WriteInt(( byte )reason);
            return p;
        }
    }
}