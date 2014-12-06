using System;
using System.IO;

namespace GameServer.Database
{
    public partial class Methods
    {
        public static void LoadItemTypes()
        {
            string[] ItemTypes = File.ReadAllLines("C:/db/itemtype.dat");
            foreach (string itemData in ItemTypes)
            {
                string[] ItemType = itemData.Split(' ');
                Items.ItemType IT = new Items.ItemType();
                IT.ID = uint.Parse(ItemType[0]);
                IT.Name = ItemType[1];
                IT.ClassReq = byte.Parse(ItemType[2]);
                IT.ProfReq = byte.Parse(ItemType[3]);
                IT.LvlReq = byte.Parse(ItemType[4]);
                IT.SexReq = byte.Parse(ItemType[5]);
                IT.StrReq = ushort.Parse(ItemType[6]);
                IT.AgiReq = ushort.Parse(ItemType[7]);
                IT.Cost = uint.Parse(ItemType[12]);
                IT.MaxAttack = ushort.Parse(ItemType[14]);
                IT.MinAttack = ushort.Parse(ItemType[15]);
                IT.Defense = ushort.Parse(ItemType[16]);
                IT.AgiGive = byte.Parse(ItemType[17]);
                IT.Dodge = byte.Parse(ItemType[18]);
                IT.VitGive = ushort.Parse(ItemType[19]);
                IT.ManaGive = ushort.Parse(ItemType[20]);
                IT.MaxDurability = ushort.Parse(ItemType[22]);
                IT.MagicAttack = ushort.Parse(ItemType[29]);
                IT.MagicDefense = byte.Parse(ItemType[30]);
                IT.Range = byte.Parse(ItemType[31]);
                Kernel.ItemTypes.TryAdd(IT.ID, IT);
            }
        }
    }
}