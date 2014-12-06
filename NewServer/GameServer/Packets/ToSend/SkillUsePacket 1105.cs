using System;
namespace GameServer.Packets
{
    public partial class ToSend
    {
        public class SkillUsePacket
        {
            private Writer Writer;
            private int offset = 20;
            public int TargetCount;
            public SkillUsePacket(uint AttackerUID, int MaxTargets)
            {
                this.Writer = new Writer(20 + (Math.Min(50, MaxTargets) * 12));
                this.Writer.Fill((ushort)1105, 2);
                this.Writer.Fill(AttackerUID, 4);
            }
            private ushort _x, _y;
            public ushort AimX { set { Writer.Fill(value, 8); _x = value; } get { return _x; } }
            public ushort AimY { set { Writer.Fill(value, 10); _y = value; } get { return _y; } }
            public ushort SkillID { set { Writer.Fill(value, 12); } }
            public byte SkillLevel { set { Writer.Fill(value, 14); } }
            public void AddTarget(uint UniqueID, uint Damage)
            {
                Writer.Fill(UniqueID, offset);
                offset += 4;
                Writer.Fill(Damage, offset);
                offset += 4;
                TargetCount++;
            }
            public byte[] ToBytes
            {
                get
                {
                    Writer.Fill((ushort)offset, 0);
                    Writer.Fill(TargetCount, 16);
                    byte[] newData = new byte[offset];
                    Buffer.BlockCopy(Writer.Bytes, 0, newData, 0, offset);
                    return newData;
                }
            }
        }
        public static byte[] SingleSpellUse(uint EntityUID, uint TargetUID, uint Damage, ushort SkillID, ushort SkillLevel, ushort X, ushort Y)
        {
            Writer PWR = new Writer(28);
            PWR.Fill((ushort)28, 0);
            PWR.Fill((ushort)1105, 2);
            PWR.Fill(EntityUID, 4);
            PWR.Fill(X, 8);
            PWR.Fill(Y, 10);
            PWR.Fill(SkillID, 12);
            PWR.Fill(SkillLevel, 14);
            PWR.Fill((uint)1, 16);
            PWR.Fill(TargetUID, 20);
            PWR.Fill(Damage, 24);
            return PWR.Bytes;
        }
    }
}
