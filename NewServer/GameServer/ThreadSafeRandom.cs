using System;

namespace GameServer
{
    public class ThreadSafeRandom
    {
        private Random rand;
        private object syncroot = new object();
        public ThreadSafeRandom()
        {
            rand = new Random(Environment.TickCount);
        }
        public int Next(int Min, int Max)
        {
            lock (syncroot)
                return rand.Next(Min, Max);
        }
    }
}
