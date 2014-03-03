#define WORLD_PREFOMANCE
#undef WORLD_PREFOMANCE

using L2.Net.GameService.InnerNetwork;
using L2.Net.GameService.WorldPlane;
using L2.Net.Structs.Services;

namespace L2.Net.GameService.Core
{
    /// <summary>
    /// Main game world class.
    /// </summary>
    internal static class World
    {
        /// <summary>
        /// Indicates if world was initialized yet.
        /// </summary>
        private static volatile bool m_Initialized;

        /// <summary>
        /// Initializes all needed world objects.
        /// </summary>
        internal static void StartUp()
        {
            if ( m_Initialized )
            {
                Logger.WriteLine(Source.World, "Can't initialize already initialized world.");
                return;
            }

            Logger.WriteLine(Source.World, "Initializing world...");

#if WORLD_PREFOMANCE
            Logger.WriteLine(Source.World, "Working set: {0}", Environment.WorkingSet);
            Stopwatch sw = new Stopwatch();
            sw.Start();
#endif
            if ( !Geodata.Initialize() )
                return;
            else
                Logger.WriteLine(Source.World, "Geodata files loaded.");

#if WORLD_PREFOMANCE
            sw.Stop();

            Logger.WriteLine(Source.World, "Working set: {0}", Environment.WorkingSet);
            Logger.WriteLine(Source.World, "GLoading time: {0}", sw.GetSplitTimeInMicroseconds());
#endif
            if ( !RegionsGrid.Initialize() )
                return;

            m_Initialized = true;

            Logger.WriteLine(Source.InnerNetwork, "Setting world as active...");

            CacheServiceConnection.Send(new SetWorldActiveRequest().ToPacket());
        }

        /// <summary>
        /// Indicates if world was initialized yet.
        /// </summary>
        internal static bool Initialized
        {
            get { return m_Initialized; }
        }
    }
}