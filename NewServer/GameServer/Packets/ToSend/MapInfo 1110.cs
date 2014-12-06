using System;

namespace GameServer.Packets
{
    public partial class ToSend
    {
        public static byte[] MapInfo(uint MapType, uint MapStatus)
        {
            Writer PWR = new Writer(16);
            PWR.Fill((ushort)16, 0);
            PWR.Fill((ushort)1110, 2);
            PWR.Fill(MapType, 4);
            PWR.Fill(MapType, 8);
            PWR.Fill(MapStatus, 12);
            return PWR.Bytes;
        }
    }
}