using System.Collections.Concurrent;
namespace GameServer.NPCs
{
    public struct NpcShop
    {
        public uint ShopID;
        public byte Type;
        public byte MoneyType;
        public byte ItemCount;
        public ConcurrentBag<uint> Items;
    }
}