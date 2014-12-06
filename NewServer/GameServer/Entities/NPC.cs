using System;

namespace GameServer.Entities
{
    public class NPC
    {
        public uint UniqueID;
        public string Name;
        public ushort Type;
        public Enums.ConquerAngle Facing;
        public ushort X;
        public ushort Y;
        public byte[] ToBytes;
    }
}
