using System;

namespace GameServer.Mapping
{
    public class MobSpawn
    {
        public uint UniqueID;
        private int X;
        private int Y;
        private int height;
        private int width;

        public MobSpawn(int x, int y, int width, int height)
        {
            this.X = x;
            this.Y = y;
            this.height = height;
            this.width = width;
        }
        public void GetLocation(out int X, out int Y)
        {
            X = Program.Random.Next(this.X, this.X + width);
            Y = Program.Random.Next(this.Y, this.Y + height);
        }
    }
}
