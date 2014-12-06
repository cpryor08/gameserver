using System;
namespace GameServer
{
    public sealed class Increments
    {
        private static uint _lastMonsterUID = 400000;
        public static uint NextMonsterUID
        {
            get { _lastMonsterUID++; return _lastMonsterUID; }
            set { _lastMonsterUID = value; }
        }
        private static uint _lastItemUID = 1;
        public static uint NextItemUID
        {
            get { _lastItemUID++; return _lastItemUID; }
            set { _lastItemUID = value; }
        }
        private static uint _nextSpawnUID = 1;
        public static uint NextSpawnUID
        {
            get { _nextSpawnUID++; return _nextSpawnUID; }
            set { _nextSpawnUID = value; }
        }
    }
}