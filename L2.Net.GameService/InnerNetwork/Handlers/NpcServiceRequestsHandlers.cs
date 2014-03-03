using L2.Net.Network;
using L2.Net.Structs.Services;

namespace L2.Net.GameService.InnerNetwork
{
    /// <summary>
    /// Npc service requests handler.
    /// </summary>
    internal sealed class NpcServiceRequestsHandlers
    {
        /// <summary>
        /// Inner network client for incoming packets handling.
        /// </summary>
        internal InnerNetworkClient Service;

        /// <summary>
        /// Initializes new instance of <see cref="NpcServiceRequestsHandlers"/> class.
        /// </summary>
        /// <param name="client">Referenced <see cref="InnerNetworkClient"/> object.</param>
        internal NpcServiceRequestsHandlers( ref InnerNetworkClient client )
        {
            Service = client;
        }

        /// <summary>
        /// Sends <see cref="Packet"/> to connected service.
        /// </summary>
        /// <param name="p"><see cref="Packet"/> to send.</param>
        internal void Send( Packet p )
        {
            if ( Service != null )
                Service.Send(p);
        }

        /// <summary>
        /// Handles incoming <see cref="Packet"/>.
        /// </summary>
        /// <param name="p"><see cref="Packet"/> to handle.</param>
        internal void Handle( Packet p )
        {
            switch ( p.FirstOpcode )
            {
                case ServiceLayer.Identity:
                    {
                        switch ( p.SecondOpcode )
                        {
                            default:
                                break;
                        }

                        break;
                    }
            }

            Logger.WriteLine("Unknown packet received: {0}", p.ToString());
        }
    }
}