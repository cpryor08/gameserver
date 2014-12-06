using System;
using System.Net.Sockets;
namespace GameServer.Network
{
    public class SocketClient
    {
        public uint UniqueID;
        public Socket Socket;
        public Cryptographer Crypto;
        public Entities.Character Character;
        public bool Connected;
        public bool Lagging;
        public void Disconnect()
        {
            Connected = false;
            try
            {
                if (Character != null)
                    Character.Save();
                Socket.Disconnect(false);
            }
            catch { }
        }
        public void Send(byte[] Data)
        {
            byte[] Dta = new byte[Data.Length];
            Buffer.BlockCopy(Data, 0, Dta, 0, Data.Length);
            Crypto.Encrypt(ref Dta);
            try { this.Socket.Send(Dta); }
            catch { Disconnect(); }
        }
    }
}
