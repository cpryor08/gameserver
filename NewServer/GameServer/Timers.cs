using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using GameServer.Network;

namespace GameServer
{
    public class Timers
    {
        public static void Init()
        {
            Task.Factory.StartNew(MobTimer);
            Task.Factory.StartNew(AutoAttackTimer);
            Task.Factory.StartNew(EventTimer);
            Task.Factory.StartNew(BuffTimer);
        }
        private static async void MobTimer()
        {
            while (true)
            {
                int Time = Environment.TickCount;
                foreach (Mapping.Map M in Kernel.Maps.Values)
                {
                    if (M.Players.Count == 0)
                        continue;
                    foreach (Entities.Monster Monster in M.Monsters.Values)
                    {
                        if (Monster.HitPoints == 0)
                        {
                            if (Monster.OnScreen && Monster.DeathStamp + 2000 < Time)
                            {
                                Monster.OnScreen = false;
                                foreach (SocketClient Client in Monster.Screen.Values)
                                    Client.Character.Screen.Remove(Monster.UniqueID);
                            }
                            else if (Monster.DeathStamp + Monster.RespawnDelay < Time)
                            {
                                Monster.HitPoints = Monster.Info.MaxHealth;
                                Monster.OnScreen = true;
                                int X, Y;
                                Monster.Spawn.GetLocation(out X, out Y);
                                Monster.X = (ushort)X;
                                Monster.Y = (ushort)Y;
                                byte[] Data = Packets.ToSend.StringPacket(Monster.UniqueID, 10, 1, "MBStandard");
                                foreach (SocketClient Client in Monster.Screen.Values)
                                {
                                    if (Calculations.GetDistance(Client.Character.X, Client.Character.Y, Monster.X, Monster.Y) <= Constants.ScreenDistance)
                                    {
                                        Client.Character.Screen.Insert(Monster);
                                        if (!Client.Lagging)
                                            Client.Send(Data);
                                    }
                                }
                            }
                        }
                        else if (Monster.Target != null)
                        {
                            if (!Monster.Target.Character.Dead)
                            {
                                double dist = Calculations.GetDistance(Monster.X, Monster.Y, Monster.Target.Character.X, Monster.Target.Character.Y);
                                if (dist <= Monster.Info.ViewRange && !Monster.Info.IsGuard)
                                {
                                    if (dist <= Monster.Info.AttackRange)
                                        Monster.Attack();
                                    else
                                    {
                                        byte ToDir = (byte)(((7 - (Math.Floor(Calculations.Direction(Monster.X, Monster.Y, Monster.Target.Character.X, Monster.Target.Character.Y) / 45 % 8)) - 1 % 8)) % 8);
                                        Monster.Walk(ToDir);
                                    }
                                }
                            }
                            else if (Monster.X != Monster.SpawnX && Monster.Y != Monster.SpawnY)
                            {
                                byte ToDir = (byte)(((7 - (Math.Floor(Calculations.Direction(Monster.X, Monster.Y, Monster.SpawnX, Monster.SpawnY) / 45 % 8)) - 1 % 8)) % 8);
                                Monster.Walk(ToDir);
                            }
                        }
                    }
                }
                await Task.Delay(1000);
            }
        }
        private static async void AutoAttackTimer()
        {
            while (true)
            {
                foreach (Mapping.Map M in Kernel.Maps.Values)
                    foreach (SocketClient Player in M.Players.Values)
                        if (Player.Character.AttackHandler.Attacking)
                            Player.Character.AttackHandler.Handle();
                await Task.Delay(100);
            }
        }
        private static async void BuffTimer()
        {
            while(true)
            {
                int CurrentTime = Environment.TickCount;
                foreach (Mapping.Map M in Kernel.Maps.Values)
                {
                    foreach (SocketClient Player in M.Players.Values)
                    {
                        foreach (Buffs.Buff B in Player.Character.Buffs.Buffs.Values)
                        {
                            if (B.NextExpiration <= CurrentTime)
                            {
                                if (B.Expires)
                                {
                                    B.Finished(Player.Character);
                                }
                                else
                                    B.NextExpiration = CurrentTime + (B.Duration * 1000);
                            }
                        }
                    }
                }
                await Task.Delay(1000);
            }
        }
        private static async void EventTimer()
        {
            while (true)
            {
                DateTime Now = DateTime.Now;
                foreach (Interface.Event Event in Kernel.Events.Values)
                {
                    if (Event.Schedule.ContainsKey(Now.DayOfWeek))
                    {
                        List<Time> Times = Event.Schedule[Now.DayOfWeek];
                        foreach (Time T in Times)
                            if (T.Hours == Now.Hour && T.Minutes == Now.Minute)
                                Event.Start();
                    }
                }
                await Task.Delay(5000);
            }
        }
    }
}
