using System;
using L2.Net.Network;

namespace L2.Net.CacheService.Network.Handlers
{
    internal static class NpcServiceRequestsHandlers
    {
        internal static void Handle( Packet p )
        {
            Logger.WriteLine(Source.InnerNetwork, "Unknown packet received from {0} service:{1}{2}", ServiceType.NpcService, Environment.NewLine, p.ToString());
        }
    }
}