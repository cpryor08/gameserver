using System;
namespace GameServer.Packets
{
    public partial class ToSend
    {
        public static byte[] AddToTeamPacket(string Name, uint UniqueID, uint Model, ushort MaxHitPoints, ushort HitPoints)
        {
            Writer PWR = new Writer(36);
            PWR.Fill((ushort)36, 0);
            PWR.Fill((ushort)1026, 2);
            PWR.Fill((byte)Name.Length, 5);
            PWR.Fill(Name, 8);
            PWR.Fill(UniqueID, 24);
            PWR.Fill(Model, 28);
            PWR.Fill(MaxHitPoints, 32);
            PWR.Fill(HitPoints, 34);
            return PWR.Bytes;
        }
    }
}