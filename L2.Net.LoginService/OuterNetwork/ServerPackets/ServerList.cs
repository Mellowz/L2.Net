using L2.Net.Network;
using L2.Net.Structs.Services;

namespace L2.Net.LoginService.OuterNetwork
{
    /// <summary>
    /// Worlds list packet.
    /// </summary>
    internal static class ServerList
    {
        /// <summary>
        /// Packet opcode.
        /// </summary>
        private const byte Opcode = 0x04;

        /// <summary>
        /// Worlds data server > client packet.
        /// </summary>
        /// <param name="lastWorld">Last world user was played in.</param>
        /// <param name="summary">Worlds summary.</param>
        /// <returns>Worlds summary <see cref="Packet"/></returns>
        internal static Packet ToPacket( byte lastWorld, WorldSummary[] summary )
        {
            Packet p = new Packet(Opcode);

            p.WriteByte
                (
                    ( byte )summary.Length,
                    lastWorld
                );

            int bits;

            foreach ( WorldSummary ws in summary )
            {
                bits = 0x00;

                if ( ws.IsTestServer )
                    bits |= 0x04;
                if ( ws.ShowClock )
                    bits |= 0x02;

                p.WriteByte(ws.ID);
                p.WriteByte(127);  // temp hack
                p.WriteByte(0);
                p.WriteByte(0);
                p.WriteByte(1);
                //p.WriteBytesArray(ws.Address);
                p.WriteInt(ws.Port);

                p.WriteByte
                    (
                        ws.AgeLimit,
                        ( ws.IsPvP ? ( byte )0x01 : ( byte )0x00 )
                    );

                p.WriteShort
                    (
                        ws.UsersOnline,
                        ws.UsersMax
                    );

                p.WriteByte(ws.IsOnline ? ( byte )0x01 : ( byte )0x00);
                p.WriteInt(bits);

                p.WriteInt(ws.ShowBrackets ? ( byte )0x01 : ( byte )0x00);
            }

            return p;
        }
    }
}