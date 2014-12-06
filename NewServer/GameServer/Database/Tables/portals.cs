using System;
using System.IO;

namespace GameServer.Database
{
    public partial class Methods
    {
        public static void LoadPortals()
        {
            string[] Portals = File.ReadAllLines("C:/db/portals.dat");
            for (int i = 0; i < Portals.Length; i++)
            {
                string[] PortalInfo = Portals[i].Split(' ');
                Mapping.Portal Portal = new Mapping.Portal();
                Portal.StartMap = uint.Parse(PortalInfo[1]);
                Portal.StartX = ushort.Parse(PortalInfo[2]);
                Portal.StartY = ushort.Parse(PortalInfo[3]);
                Portal.EndMap = uint.Parse(PortalInfo[4]);
                Portal.EndX = ushort.Parse(PortalInfo[5]);
                Portal.EndY = ushort.Parse(PortalInfo[6]);
                Mapping.Map M;
                if (Kernel.Maps.TryGetValue(Portal.StartMap, out M))
                    M.Portals.Add(Portal);
            }
        }
    }
}