using System;

namespace GameServer.Packets
{
    public partial class ToSend
    {
        public static byte[] Attack(uint AttackerUID, uint AttackedUID, ushort AttackedX, ushort AttackedY, uint Damage, Enums.AttackType AttackType)
        {
            Writer PWR = new Writer(28);
            PWR.Fill((ushort)28, 0);
            PWR.Fill((ushort)1022, 2);
            PWR.Fill(AttackerUID, 8);
            PWR.Fill(AttackedUID, 12);
            PWR.Fill(AttackedX, 16);
            PWR.Fill(AttackedY, 18);
            PWR.Fill((uint)AttackType, 20);
            PWR.Fill(Damage, 24);
            return PWR.Bytes;
        }
    }
}
