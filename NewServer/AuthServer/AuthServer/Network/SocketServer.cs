using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

namespace AuthServer.Network
{
    public class SocketServer
    {
        private SocketAsyncEventArgsPool readWritePool;
        private Socket listenSocket;
        public SocketServer()
        {
            readWritePool = new SocketAsyncEventArgsPool(Constants.MaxConnections);
            SocketAsyncEventArgs readWriteEventArg;
            for (int i = 0; i < Constants.MaxConnections; i++)
            {
                readWriteEventArg = new SocketAsyncEventArgs();
                readWriteEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(IO_Completed);
                readWriteEventArg.SetBuffer(new byte[Constants.BufferSize], 0, Constants.BufferSize);
                readWriteEventArg.UserToken = new SocketClient();
                readWritePool.Push(readWriteEventArg);
            }
        }
        private void IO_Completed(object sender, SocketAsyncEventArgs e)
        {
            switch(e.LastOperation)
            {
                case SocketAsyncOperation.Receive: ProcessReceive(e); break;
                default: throw new ArgumentException("The last operation completed on the socket was not a receive or send");
            }
        }
        public void Start(string IP, ushort Port)
        {
            IPEndPoint ipe = new IPEndPoint(IPAddress.Parse(IP), Port);
            listenSocket = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            listenSocket.ReceiveBufferSize = Constants.BufferSize;
            listenSocket.SendBufferSize = Constants.BufferSize;
            listenSocket.SendTimeout = 2000;
            listenSocket.ReceiveTimeout = 2000;
            listenSocket.Bind(ipe);
            listenSocket.Listen(Constants.MaxConnections);
            StartAccept(null);
        }
        private void StartAccept(SocketAsyncEventArgs acceptEventArgs)
        {
            if (acceptEventArgs == null)
            {
                acceptEventArgs = new SocketAsyncEventArgs();
                acceptEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(ProcessAccept);
            }
            else
                acceptEventArgs.AcceptSocket = null;
            bool handle = listenSocket.AcceptAsync(acceptEventArgs);
            if (!handle)
                ProcessAccept(null, acceptEventArgs);
        }
        private void ProcessAccept(object sender, SocketAsyncEventArgs e)
        {
            SocketAsyncEventArgs readEventArgs = readWritePool.Pop();
            ((SocketClient)readEventArgs.UserToken).Socket = e.AcceptSocket;
            try
            {
                bool handle = e.AcceptSocket.ReceiveAsync(readEventArgs);
                if (!handle)
                    ProcessReceive(readEventArgs);
                StartAccept(e);
            }
            catch (Exception x) { Console.WriteLine(x.ToString()); }
        }
        private void ProcessReceive(SocketAsyncEventArgs e)
        {
            SocketClient client = (SocketClient)e.UserToken;
            if (e.BytesTransferred > 0 && e.SocketError == SocketError.Success)
            {
                byte[] Received = new byte[e.BytesTransferred];
                Buffer.BlockCopy(e.Buffer, 0, Received, 0, e.BytesTransferred);
                Packets.Handler.Handle(Received, client);
            }
            else
                ProcessDisconnect(e);
        }
        private void ProcessDisconnect(SocketAsyncEventArgs e)
        {
            SocketClient client = e.UserToken as SocketClient;
            try
            { client.Socket.Shutdown(SocketShutdown.Send); } catch (Exception) { }
            client.Socket.Close();
            readWritePool.Push(e);
        }
    }
}
