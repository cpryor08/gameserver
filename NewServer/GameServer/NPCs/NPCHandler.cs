using System;
using System.Text;
using GameServer.Network;

namespace GameServer.NPCs
{
    public class NPCHandler
    {
        private SocketClient Owner;
        public uint ActiveNPC = 0;
        public byte LastOption = 0;
        public string LastInput = "";
        public NPCHandler(SocketClient Owner) { this.Owner = Owner; }

        public void Handle(byte[] Data)
        {
            byte OptionID = Data[10];
            string CurrentInput = Encoding.ASCII.GetString(Data, 14, Data[13]);

            if (OptionID == 255)
            {
                Reset();
                return;
            }

            switch(ActiveNPC)
            {
                case 731:
                    {
                        Owner.Send(Packets.ToSend.ItemUsage(Owner.UniqueID, (byte)Enums.ItemUsage.ViewWarehouse, Owner.Character.StoredSilver, 0));
                        Owner.Send(Packets.ToSend.GeneralData(Owner.UniqueID, 4, 0, 0, 0, Enums.GeneralData.Dialog));
                        break;
                    }
                default:
                    {
                        Text("This NPC is not complete. Please try again later. NpcID: " + ActiveNPC);
                        Link("Ok, thanks.", 255);
                        End();
                        break;
                    }
            }

            LastInput = CurrentInput;
        }

        public void Reset()
        {
            this.ActiveNPC = 0;
            this.LastOption = 0;
            this.LastInput = "";
        }
        public void Text(string Msg)
        {
            Owner.Send(Packets.ToSend.NPCTalk(255, 1, Msg));
        }
        public void Link(string value, byte LinkBack)
        {
            Owner.Send(Packets.ToSend.NPCTalk(LinkBack, 2, value));
        }
        public void Input(byte LinkBack, string FieldName)
        {
            Owner.Send(Packets.ToSend.NPCTalk(LinkBack, 3, FieldName));
        }
        public void End()
        {
            Owner.Send(Packets.ToSend.NPCTalk(0, 0, 255, 100));
        }
        public void Face(int Face)
        {
            Owner.Send(Packets.ToSend.NPCTalk(2544, Face, 255, 4));
        }
    }
}
