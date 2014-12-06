using System;
using GameServer.Network;

namespace GameServer.Packets.ToReceive
{
    public class GeneralData : Packet
    {
        public int MinLength { get { return 22; } }
        public void Handle(byte[] Data, SocketClient Client)
        {
            byte Type = Data[22];
            switch(Type)
            {
                case 74:
                    {
                        Calculations.CalculateMaxHP(Client.Character);
                        Calculations.CalculateMaxMana(Client.Character);
                        Client.Send(Packets.ToSend.GeneralData(Client.UniqueID, Client.Character.MapID, Client.Character.X, Client.Character.Y, 0, Enums.GeneralData.SetLocation));
                        Client.Send(Packets.ToSend.GeneralData(Client.UniqueID, Client.Character.MapID, Client.Character.X, Client.Character.Y, 0, Enums.GeneralData.ChgMap));
                        Client.Send(Packets.ToSend.MapInfo(Client.Character.MapID, (uint)(Client.Character.MapID == 1036 ? 30 : 2080)));
                        Client.Character.Map.Insert(Client);
                        break;
                    }
                case 79:
                    {
                        Client.Character.Facing = (Enums.ConquerAngle)Data[20];
                        break;
                    }
                case 133:
                    {
                        ushort new_X = BitConverter.ToUInt16(Data, 12);
                        ushort new_Y = BitConverter.ToUInt16(Data, 14);
                        double jumpDistance = Calculations.GetDistance(Client.Character.X, Client.Character.Y, new_X, new_Y);
                        if (jumpDistance <= Constants.ScreenDistance)
                        {
                            Client.Character.X = new_X;
                            Client.Character.Y = new_Y;
                            Client.Character.Screen.Send(Data, true);
                            Client.Character.Map.PopulateScreen(Client);
                        }
                        else
                            Client.Disconnect();
                        break;
                    }
                case 142:
                    {
                        if (Client.Character.Silver >= 500)
                        {
                            Client.Character.Silver -= 500;
                            Client.Character.Avatar = BitConverter.ToUInt16(Data, 12);
                        }
                        else
                            Client.Send(Packets.ToSend.MessagePacket("You don't have 500 silvers.", "SYSTEM", Client.Character.Name, Enums.ChatType.Top));
                        break;
                    }
                default: Console.WriteLine("Missing General Data PacketID: {0}", Type); break;
            }
        }
    }
}
