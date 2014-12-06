using System;
using GameServer.Network;

namespace GameServer.Packets.ToReceive
{
    public class NewCharacter : Packet
    {
        public int MinLength { get { return 60; } }
        public void Handle(byte[] Data, SocketClient Client)
        {
            string CharName = System.Text.ASCIIEncoding.ASCII.GetString(Data, 20, 16).Replace("\0", "");
            ushort Model = BitConverter.ToUInt16(Data, 52);
            ushort Class = BitConverter.ToUInt16(Data, 54);
            uint AccountID = BitConverter.ToUInt32(Data, 56);

            if (Model != 1003 && Model != 1004 && Model != 2001 && Model != 2002)
            {
                Client.Disconnect();
                return;
            }
            if (Class != 10 && Class != 100 && Class != 20 && Class != 40)
            {
                Client.Disconnect();
                return;
            }
            if (Database.Methods.ValidString(CharName))
            {
                if (!Database.Methods.NameExists(CharName))
                {
                    if (!Database.Methods.HasCharacter(Client.UniqueID))
                    {
                        Database.Methods.CreateCharacter(Client.UniqueID, CharName, Class, Model);
                        Client.Send(Packets.ToSend.MessagePacket("ANSWER_OK", "SYSTEM", "ALLUSERS", Enums.ChatType.Dialog));
                        Client.Disconnect();
                    }
                    else
                        Client.Send(Packets.ToSend.MessagePacket("You already have a character created.", "SYSTEM", "ALLUSERS", Enums.ChatType.Dialog));
                }
                else
                    Client.Send(Packets.ToSend.MessagePacket("Character name has been taken! Please choose a new one!", "SYSTEM", "ALLUSERS", Enums.ChatType.Dialog));
            }
            else
                Client.Send(Packets.ToSend.MessagePacket("Invalid character name!", "SYSTEM", "ALLUSERS", Enums.ChatType.Dialog));
        }
    }
}
