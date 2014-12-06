using System;

namespace GameServer.Mapping
{
    public struct Portal
    {
        public uint StartMap;
        public ushort StartX;
        public ushort StartY;
        public uint EndMap;
        public ushort EndX;
        public ushort EndY;
    }
}
