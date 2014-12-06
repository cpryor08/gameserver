using System;
using GameServer.Packets;
using GameServer.Network;
using System.Collections.Concurrent;

namespace GameServer.Entities
{
    public class Monster
    {
        public MonsterType Info;
        public Mapping.MobSpawn Spawn;
        private Writer packetWriter;
        public Monster(MonsterType Info, Mapping.MobSpawn Spawn)
        {
            this.Info = Info;
            this.Spawn = Spawn;
            UniqueID = Increments.NextMonsterUID;
            packetWriter = new Writer(82 + Info.Name.Length);
            packetWriter.Fill((ushort)82 + Info.Name.Length, 0);
            packetWriter.Fill((ushort)1014, 2);
            packetWriter.Fill(UniqueID, 4);
            packetWriter.Fill(Info.Model, 8);
            packetWriter.Fill(Info.Level, 50);
            packetWriter.Fill((byte)0x64, 59);
            packetWriter.Fill((byte)1, 80);
            packetWriter.Fill((byte)Info.Name.Length, 81);
            packetWriter.Fill(Info.Name, 82);
            HitPoints = Info.MaxHealth;
            Facing = (Enums.ConquerAngle)Program.Random.Next(0, 7);
        }
        public uint UniqueID;

        public ConcurrentDictionary<uint, SocketClient> Screen = new ConcurrentDictionary<uint, SocketClient>();
        public bool OnScreen = true;
        public int DeathStamp;
        public int RespawnDelay;

        private ushort _hitpoints;
        public ushort HitPoints
        {
            get { return _hitpoints; }
            set
            {
                _hitpoints = value;
                packetWriter.Fill(_hitpoints, 48);
            }
        }

        public Mapping.Map Map;
        private ushort _x;
        public ushort X
        {
            get { return _x; }
            set
            {
                _x = value;
                packetWriter.Fill(_x, 52);
            }
        }
        private ushort _y;
        public ushort Y
        {
            get { return _y; }
            set
            {
                _y = value;
                packetWriter.Fill(_y, 54);
            }
        }

        public ushort SpawnX;
        public ushort SpawnY;

        #region Buff Management
        public long Flags;

        public Boolean ContainsFlag(long Flag) { return (Flags & Flag) != 0; }
        public void AddFlag(long Flag) { Flags |= Flag; }
        public void DelFlag(long Flag) { Flags &= ~Flag; }

        public Boolean ContainsFlag(Enums.Flag Flag) { return (Flags & (long)Flag) != 0; }
        public void AddFlag(Enums.Flag Flag) { Flags |= (long)Flag; }
        public void DelFlag(Enums.Flag Flag) { Flags &= ~(long)Flag; }
        #endregion

        public SocketClient Target;

        private Enums.ConquerAngle _facing = Enums.ConquerAngle.SouthWest;
        public Enums.ConquerAngle Facing
        {
            get { return _facing; }
            set
            {
                _facing = value;
                packetWriter.Fill((byte)_facing, 58);
            }
        }

        public void Walk(byte ToDir)
        {
            if (ToDir == 255)
                ToDir = 7;
            ushort NewX = X;
            ushort NewY = Y;
            switch (ToDir)
            {
                case 0:
                    {
                        NewY++;
                        break;
                    }
                case 1:
                    {
                        NewX--;
                        NewY++;
                        break;
                    }
                case 2:
                    {
                        NewX--;
                        break;
                    }
                case 3:
                    {
                        NewX--;
                        NewY--;
                        break;
                    }
                case 4:
                    {
                        NewY--;
                        break;
                    }
                case 5:
                    {
                        NewX++;
                        NewY--;
                        break;
                    }
                case 6:
                    {
                        NewX++;
                        break;
                    }
                case 7:
                    {
                        NewY++;
                        NewX++;
                        break;
                    }
            }
            X = NewX;
            Y = NewY;
            SendScreen(Packets.ToSend.MobMove(UniqueID, ToDir));
        }
        public void Attack()
        {
            SocketClient target = this.Target;
        }
        public void TakeDamage(uint AttackerUID, uint Damage)
        {
            if (HitPoints > Damage)
                HitPoints = (ushort)(HitPoints - Damage);
            else
            {
                this.Target = null;
                DeathStamp = Environment.TickCount;
                HitPoints = 0;
                SendScreen(Packets.ToSend.Attack(AttackerUID, UniqueID, X, Y, 0, Enums.AttackType.Death));
                SendScreen(Packets.ToSend.SyncPacket(Target.UniqueID, Enums.SyncType.RaiseFlag, 2080));
            }
        }

        public void SendScreen(byte[] Data)
        {
            foreach (SocketClient client in Screen.Values)
                client.Send(Data);
        }
        public byte[] ToBytes() { return packetWriter.Bytes; }
    }
}
