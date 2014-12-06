using System;

namespace GameServer.Enums
{
    public enum ItemUsage : byte
    {
        BuyFromNPC = 1,
        SellToNPC = 2,
        RemoveInventory = 3,
        EquipItem = 4,
        UnequipItem = 6,
        ViewWarehouse = 9,
        WarehouseDeposit = 10,
        WarehouseWithdraw = 11,
        DropMoney = 12,
        Unknown13 = 13,
        RepairItem = 14,
        Unknown15 = 15,
        Unknown16 = 16,
        Unknown17 = 17,
        Unknown18 = 18,
        DragonBallUpgrade = 19,
        MeteorUpgrade = 20,
        Unknown21 = 21,
        AddStallItemForSilver = 22,
        RemoveStallItem = 23,
        BuyFromMarketShop = 24,
        Unknown25 = 25,
        Unknown26 = 26,
        Ping = 27,
        Unknown28 = 28,
        AddStallItemForCPs = 29
    }
}