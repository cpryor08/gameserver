using System;

namespace GameServer.Enums
{
    public static class ItemMode
    {
        public const ushort
            None = 0x00,
            Default = 0x01,
            Trade = 0x02,
            UpdateItem = 0x03,
            View = 0x04;
    }
}
