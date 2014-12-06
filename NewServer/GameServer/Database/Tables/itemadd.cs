using System;
using System.IO;
using System.Collections.Concurrent;

namespace GameServer.Database
{
    public partial class Methods
    {
        public static void LoadItemPlusInfo()
        {
            string[] IPInfo = File.ReadAllLines("C:/db/itemadd.dat");
            foreach (string plusInfo in IPInfo)
            {
                string[] Info = plusInfo.Split(' ');
                Items.ItemPlusInfo PInfo = new Items.ItemPlusInfo();
                PInfo.ID = uint.Parse(Info[0]);
                PInfo.Plus = byte.Parse(Info[1]);
                PInfo.HP = ushort.Parse(Info[2]);
                PInfo.MinAtk = int.Parse(Info[3]);
                PInfo.MaxAtk = int.Parse(Info[4]);
                PInfo.Defense = ushort.Parse(Info[5]);
                PInfo.MAtk = int.Parse(Info[6]);
                PInfo.MDef = ushort.Parse(Info[7]);
                PInfo.Dex = ushort.Parse(Info[8]);
                PInfo.Dodge = byte.Parse(Info[9]);
                Kernel.ItemPlusInfos.TryAdd(PInfo.ID, new ConcurrentDictionary<byte, Items.ItemPlusInfo>());
                Kernel.ItemPlusInfos[PInfo.ID].TryAdd(PInfo.Plus, PInfo);
            }
        }
    }
}