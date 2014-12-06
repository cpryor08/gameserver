using System;
using System.IO;
using System.Collections.Concurrent;
using GameServer.Network;

namespace GameServer.Mapping
{
    public class Map
    {
        public uint UniqueID;
        public uint Type;

        public int Width;
        public int Height;

        public ConcurrentDictionary<uint, SocketClient> Players = new ConcurrentDictionary<uint, SocketClient>();
        public ConcurrentDictionary<uint, Entities.Monster> Monsters = new ConcurrentDictionary<uint, Entities.Monster>();
        public ConcurrentDictionary<uint, Entities.NPC> NPCs = new ConcurrentDictionary<uint, Entities.NPC>();
        public ConcurrentDictionary<uint, Items.FloorItem> FloorItems = new ConcurrentDictionary<uint, Items.FloorItem>();
        public ConcurrentBag<Mapping.Portal> Portals = new ConcurrentBag<Portal>();
        public object syncroot = new object();
        public byte[,] Coords;

        public Map() { }
        public void Insert(SocketClient Client)
        {
            if (Players.TryAdd(Client.UniqueID, Client))
                PopulateScreen(Client);
        }
        public void Insert(Entities.Monster Mob)
        {
            if (Monsters.TryAdd(Mob.UniqueID, Mob))
                foreach (SocketClient client in Players.Values)
                    if (Calculations.GetDistance(client.Character.X, client.Character.Y, Mob.X, Mob.Y) <= Constants.ScreenDistance)
                        client.Character.Screen.Insert(Mob);
        }
        public void Insert(Entities.NPC Npc)
        {
            if (NPCs.TryAdd(Npc.UniqueID, Npc))
                foreach (SocketClient client in Players.Values)
                    if (Calculations.GetDistance(client.Character.X, client.Character.Y, Npc.X, Npc.Y) <= Constants.ScreenDistance)
                        client.Character.Screen.Insert(Npc);
        }
        public void Remove(uint UniqueID)
        {
            SocketClient removedClient;
            if (Players.TryRemove(UniqueID, out removedClient))
                removedClient.Character.Screen.Clear();
        }
        public void PopulateScreen(SocketClient Client)
        {
            foreach (SocketClient nC in Players.Values)
            {
                if (Calculations.GetDistance(nC.Character.X, nC.Character.Y, Client.Character.X, Client.Character.Y) <= Constants.ScreenDistance)
                {
                    Client.Character.Screen.Insert(nC);
                    nC.Character.Screen.Insert(Client);
                }
                else
                {
                    Client.Character.Screen.Remove(nC.UniqueID);
                    nC.Character.Screen.Remove(Client.UniqueID);
                }
            }
            foreach (Entities.Monster Monster in Monsters.Values)
            {
                if (Monster.HitPoints > 0)
                {
                    if (Calculations.GetDistance(Monster.X, Monster.Y, Client.Character.X, Client.Character.Y) <= Constants.ScreenDistance)
                        Client.Character.Screen.Insert(Monster);
                    else
                        Client.Character.Screen.Remove(Monster.UniqueID);
                }
            }
            foreach (Entities.NPC Npc in NPCs.Values)
            {
                if (Calculations.GetDistance(Npc.X, Npc.Y, Client.Character.X, Client.Character.Y) <= Constants.ScreenDistance)
                    Client.Character.Screen.Insert(Npc);
                else
                    Client.Character.Screen.Remove(Npc.UniqueID);
            }
        }
        public void DropAnItem(ushort X, ushort Y, byte Level, uint OwnerUID)
        {
            lock (syncroot)
            {
                for (short x = -1; x < 2; x++)
                {
                    for (short y = -1; y < 2; y++)
                    {
                        ushort nX = (ushort)(X + x);
                        ushort nY = (ushort)(Y + y);
                        if (Coords[nX, nY] < 3)
                        {
                            Coords[nX, nY] += 2;
                            Items.FloorItem Itm = new Items.FloorItem();
                            Itm.ItemID = (uint)Calculations.GetDropID(Level);
                            Itm.OwnerUID = OwnerUID;
                            Itm.DropTime = Environment.TickCount;
                            Itm.Plus = Calculations.GetPlus();
                            Itm.UniqueID = Increments.NextItemUID;
                            Itm.X = nX;
                            Itm.Y = nY;
                            FloorItems.TryAdd(Itm.UniqueID, Itm);
                            foreach (SocketClient Client in Players.Values)
                                if (Calculations.GetDistance(Client.Character.X, Client.Character.Y, nX, nY) <= Constants.ScreenDistance)
                                    Client.Character.Screen.Insert(Itm);
                            return;
                        }
                    }
                }
            }
        }
        public bool LoadDMap()
        {
            if (File.Exists("C:/db/DMap/Maps/" + Type + ".JMap"))
            {
                using (BinaryReader Reader = new BinaryReader(new FileStream("C:/db/DMap/Maps/" + Type + ".JMap", FileMode.Open)))
                {
                    Width = Reader.ReadInt16();
                    Height = Reader.ReadInt16();
                    Coords = new byte[Height, Width];

                    for (short ix = 0; ix < Height; ix++)
                        for (short iy = 0; iy < Width; iy++)
                            Coords[ix, iy] = (byte)(Reader.ReadByte() == 1 ? 255 : 0);
                    Reader.BaseStream.Close();
                    Reader.Close();
                }
                return true;
            }
            return false;
        }
        public static void Init()
        {
            ConcurrentDictionary<uint, ushort[]> dimensions = new ConcurrentDictionary<uint, ushort[]>();
            string[] dimensionData = System.IO.File.ReadAllLines("C:/db/mapdimensions.dat");
            foreach (string md in dimensionData)
            {
                string[] mdd = md.Split(' ');
                ushort Width = ushort.Parse(mdd[1]);
                ushort Height = ushort.Parse(mdd[2]);
                ushort[] dim = new ushort[2] { Width, Height };
                dimensions.TryAdd(uint.Parse(mdd[0]), dim);
            }
            Database.Database DB = new Database.Database("C:/db/maps.s3db");
            System.Data.DataTable DT = DB.GetDataTable("SELECT `UniqueID`, `Type` FROM `maptypes`");
            for (int i = 0; i < DT.Rows.Count; i++)
            {
                System.Data.DataRow dr = DT.Rows[i];
                Mapping.Map M = new Map();
                M.UniqueID = Convert.ToUInt32(dr.ItemArray[0]);
                M.Type = Convert.ToUInt32(dr.ItemArray[1]);
                if (!M.LoadDMap())
                {
                    ushort[] dims;
                    if (dimensions.TryGetValue(M.Type, out dims))
                    {
                        M.Width = dims[0];
                        M.Height = dims[1];
                    }
                    else
                    {
                        M.Width = 1300;
                        M.Height = 1300;
                    }
                    M.Coords = new byte[M.Height, M.Width];
                    for (short ix = 0; ix < M.Height; ix++)
                        for (short iy = 0; iy < M.Width; iy++)
                            M.Coords[ix, iy] = (byte)Enums.MapPointType.Empty;
                }
                Kernel.Maps.TryAdd(M.UniqueID, M);
            }
            DB.Dispose();
            dimensions.Clear();
        }
    }
}
