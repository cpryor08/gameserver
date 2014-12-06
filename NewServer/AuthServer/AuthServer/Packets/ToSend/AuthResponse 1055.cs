using System;
namespace AuthServer.Packets
{
    public partial class ToSend
    {
        public static byte[] AuthResponse(Enums.AuthResponseType Art, uint UniqueID)
        {
            Writer PWR = new Writer(32);
            PWR.Fill((ushort)32, 0);
            PWR.Fill((ushort)1055, 2);
            PWR.Fill((uint)Art, 8);
            if (Art == Enums.AuthResponseType.Successful)
            {
                PWR.Fill((uint)UniqueID, 4);
                PWR.Fill(Constants.IPAddress, 12);
                PWR.Fill((ushort)5816, 28);
            }
            else
                PWR.Fill((uint)0, 4);
            return PWR.Bytes;
        }
    }
}
