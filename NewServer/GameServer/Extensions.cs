using System;
using System.Collections.Generic;

namespace GameServer
{
    public static class Extensions
    {
        public static bool Contains(this List<Calculations.coords> coordList, ushort X, ushort Y)
        {
            foreach (Calculations.coords cord in coordList)
                if (cord.X == X && cord.Y == Y)
                    return true;
            return false;
        }
    }
}
