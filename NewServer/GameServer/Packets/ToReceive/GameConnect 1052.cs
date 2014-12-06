using System;
using GameServer.Network;

namespace GameServer.Packets.ToReceive
{
    public class GameConnect : Packet
    {
        public int MinLength { get { return 28; } }
        public void Handle(byte[] Data, SocketClient Client)
        {
            Client.UniqueID = BitConverter.ToUInt32(Data, 4);
            if (Database.Methods.Authenticated(Client.UniqueID))
            {
                Client.Crypto.SetKeys(255, Client.UniqueID);
                if (Database.Methods.LoadCharacter(Client))
                {
                    Client.Connected = true;
                    Database.Methods.LoadItems(Client);
                    Database.Methods.LoadProfs(Client);
                    Client.Send(Packets.ToSend.MessagePacket("ANSWER_OK", "SYSTEM", "ALLUSERS", Enums.ChatType.LoginInformation));
                    Client.Send(Packets.ToSend.CharacterInfo(Client));
                }
                else
                    Client.Send(Packets.ToSend.MessagePacket("NEW_ROLE", "SYSTEM", "ALLUSERS", Enums.ChatType.LoginInformation));
            }
            else
            {
                Client.Send(Packets.ToSend.MessagePacket("Nice try buddy <3", "SYSTEM", "ALLUSERS", Enums.ChatType.LoginInformation));
                Client.Disconnect();
            }
        }
    }
}
