using System;
using GameServer.Network;
using System.Threading.Tasks;

namespace GameServer
{
    class Program
    {
        public static ThreadSafeRandom Random = new ThreadSafeRandom();
        static void Main(string[] args)
        {
            Console.Title = "Game Server";
            int time = Environment.TickCount;

            Database.Database.Init();
            Packets.Handler.Init();
            Mapping.Map.Init();
            Database.Methods.LoadPortals();
            Database.Methods.LoadItemTypes();
            Database.Methods.LoadItemPlusInfo();
            Database.Methods.LoadMonsterTypes();
            Database.Methods.LoadMonsterSpawns();
            Database.Methods.LoadNPCs();
            Database.Methods.LoadShops();
            Database.Methods.LoadSkills();
            Database.Methods.LoadWarehouseKeys();
            Timers.Init();

            Console.WriteLine("Load Time: " + (Environment.TickCount - time) + " ms");

            GC.Collect();

            Network.SocketServer gameServer = new Network.SocketServer();
            gameServer.Start(Constants.IPAddress, Constants.Port);

            Console.WriteLine("Listening for connections.");

            while (true) Console.ReadKey();
        }
    }
}
