using System;
using AuthServer.Network;
using System.Data;
namespace AuthServer.Packets
{
    public class Handler
    {
        public static void Handle(byte[] Data, SocketClient Client)
        {
            Client.Crypto = new Cryptographer();
            Client.Crypto.Decrypt(ref Data);
            ushort PacketID = BitConverter.ToUInt16(Data, 2);
            if (PacketID == 1051)
            {
                string Account = System.Text.ASCIIEncoding.ASCII.GetString(Data, 4, 16).Replace("\0", "");
                byte[] PassBytes = new byte[16];
                Buffer.BlockCopy(Data, 20, PassBytes, 0, 16);
                string Password = RC5.Decrypt(PassBytes);
                DataTable DT = Program.AccountDB.GetDataTable("SELECT `uid`, `type` FROM `userinfo` WHERE `username`='" + Account + "' AND `password`='" + Password + "'");
                if (DT.Rows.Count == 1)
                {
                    DataRow dr = DT.Rows[0];
                    uint UID = Convert.ToUInt32(dr.ItemArray[0]);
                    Enums.AccountType AcctType = (Enums.AccountType)Convert.ToByte(dr.ItemArray[1]);
                    if (AcctType == Enums.AccountType.BannedAccount)
                        Client.Send(Packets.ToSend.AuthResponse(Enums.AuthResponseType.Banned, UID));
                    else
                    {
                        string date = DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + " " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
                        Program.AccountDB.ExecuteNonQuery("INSERT INTO `login_tokens` (`PlayerUID`, `Date`) VALUES ('" + UID + "', '" + date + "')");
                        Client.Send(Packets.ToSend.AuthResponse(Enums.AuthResponseType.Successful, UID));
                    }
                }
                else
                    Client.Send(Packets.ToSend.AuthResponse(Enums.AuthResponseType.WrongPass, 0));
            }
            Client.Disconnect();
        }
    }
}
