using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace L2.Net.GameService.WorldPlane
{
    internal static class RegionsGrid
    {
        //private static readonly SortedList<int, Region> m_Regions = new SortedList<int, Region>();


        internal static bool Initialize()
        {
            try
            {
                return true;
                Logger.Write(false, "Initializing regions grid... ");

                Logger.EndWrite("complete.");
            }
            catch ( Exception e )
            {
                Logger.Exception(e);
                return false;
            }
        }
    }
}