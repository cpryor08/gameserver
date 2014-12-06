using System;
using System.Collections.Concurrent;
namespace GameServer.Database
{
    public partial class Methods
    {
        public static void LoadWarehouseKeys()
        {
            Kernel.WarehouseKeys.TryAdd(1002, 245); //TC
            Kernel.WarehouseKeys.TryAdd(1015, 246); //BI
            Kernel.WarehouseKeys.TryAdd(1020, 247); //AC
            Kernel.WarehouseKeys.TryAdd(1036, 248); //Market
            Kernel.WarehouseKeys.TryAdd(1000, 249); //DC
            Kernel.WarehouseKeys.TryAdd(1011, 250); //PC
        }
    }
}