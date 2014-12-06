using System;

namespace AuthServer
{
    class Program
    {
        public static Database AccountDB;
        static void Main(string[] args)
        {
            Console.Title = "Auth Server";

            AccountDB = new Database("C:/db/accounts.s3db");

            Network.SocketServer authServer = new Network.SocketServer();
            authServer.Start(Constants.IPAddress, Constants.Port);

            Console.WriteLine("Listening for connections.");

            while (true)
                Console.ReadKey();
        }
    }
}
