using System;
using GameServer.Network;

namespace GameServer.Packets
{
    public partial class ToSend
    {
        public static byte[] CharacterInfo(SocketClient Client)
        {
            int Length = 74 + 0 + Client.Character.Name.Length + 1;
            Writer PWR = new Writer(Length);
            PWR.Fill((ushort)Length, 0);
            PWR.Fill((ushort)1006, 2);
            PWR.Fill(Client.UniqueID, 4);
            PWR.Fill(Client.Character.FullModel, 8);
            PWR.Fill(Client.Character.HairStyle, 12);
            PWR.Fill(Client.Character.Silver, 14);
            PWR.Fill(Client.Character.CPs, 18);
            PWR.Fill(Client.Character.Experience, 22);
            PWR.Fill((ushort)5130, 42);
            PWR.Fill(Client.Character.Strength, 46);
            PWR.Fill(Client.Character.Agility, 48);
            PWR.Fill(Client.Character.Vitality, 50);
            PWR.Fill(Client.Character.Spirit, 52);
            PWR.Fill(Client.Character.StatPoints, 54);
            PWR.Fill(Client.Character.HitPoints, 56);
            PWR.Fill(Client.Character.Mana, 58);
            PWR.Fill(Client.Character.PKPoints, 60);
            PWR.Fill(Client.Character.Level, 62);
            PWR.Fill(Client.Character.Class, 63);
            PWR.Fill((byte)5, 64);
            PWR.Fill(Client.Character.RBCount, 65);
            PWR.Fill((byte)1, 66);
            PWR.Fill((byte)2, 67);
            PWR.Fill((byte)Client.Character.Name.Length, 68);
            PWR.Fill(Client.Character.Name, 69);
            return PWR.Bytes;
        }
    }
}