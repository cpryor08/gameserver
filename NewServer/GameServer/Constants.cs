using System;
namespace GameServer
{
    public static class Constants
    {
        public const int MaxConnections = 300;
        public const int BufferSize = 1024;
        public const string IPAddress = "127.0.0.1";
        public const string CommandOperator = "/";
        public const ushort Port = 5816;
        public const byte ScreenDistance = 18;

        public const uint Min_MobUID = 400001;
        public const uint Max_MobUID = 499999;

        public const uint Min_PlayerUID = 1000000;
        public const uint Max_PlayerUID = 1999999999;

        public const uint MaxMoney = 999999999;
    }
}
