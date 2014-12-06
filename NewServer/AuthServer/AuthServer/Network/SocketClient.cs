using System;
using System.Net.Sockets;
namespace AuthServer.Network
{
    public class SocketClient
    {
        public Socket Socket;
        public Cryptographer Crypto;
        public void Disconnect()
        {
            Socket.Disconnect(false);
        }
        public void Send(byte[] Data)
        {
            byte[] Dta = new byte[Data.Length];
            Buffer.BlockCopy(Data, 0, Dta, 0, Data.Length);
            lock (Crypto)
                Crypto.Encrypt(ref Dta);
            try { this.Socket.Send(Dta); }
            catch { Disconnect(); }
        }
    }
}
