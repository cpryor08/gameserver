using System;
using System.Collections.Concurrent;
namespace GameServer
{
    public class Team
    {
        private uint TeamID = 0;
        private ConcurrentDictionary<uint, Network.SocketClient> Members = new ConcurrentDictionary<uint, Network.SocketClient>();
        public Team(Network.SocketClient Owner)
        {
            this.TeamID = Owner.UniqueID;
            if (Members.TryAdd(Owner.UniqueID, Owner))
            {
                Owner.Send(Packets.ToSend.TeamPacket(Owner.UniqueID, Enums.TeamPacket.Create));
            }
        }
        public bool ContainsMember(uint UniqueID)
        {
            return Members.ContainsKey(UniqueID);
        }
        public void AddMember(Network.SocketClient NewMember)
        {
            if (Members.TryAdd(NewMember.UniqueID, NewMember))
            {
                byte[] Data = Packets.ToSend.AddToTeamPacket(NewMember.Character.Name, NewMember.UniqueID, NewMember.Character.FullModel, NewMember.Character.MaxHitPoints, NewMember.Character.HitPoints);
                foreach (Network.SocketClient Member in Members.Values)
                    Member.Send(Data);
            }
        }
        public void RemoveMember(uint UniqueID)
        {
            Network.SocketClient SC;
            if (Members.TryRemove(UniqueID, out SC))
            {
                if (SC.Connected)
                    SC.Send(Packets.ToSend.TeamPacket(UniqueID, Enums.TeamPacket.Kick));
                foreach (Network.SocketClient Member in Members.Values)
                    Member.Send(Packets.ToSend.TeamPacket(UniqueID, Enums.TeamPacket.ExitTeam));
            }
        }
        public void Dismiss()
        {
            byte[] Data = Packets.ToSend.TeamPacket(0, Enums.TeamPacket.Dismiss);
            foreach (Network.SocketClient Member in Members.Values)
            {
                Network.SocketClient _c;
                if (Members.TryRemove(Member.UniqueID, out _c))
                {
                    _c.Character.TeamID = 0;
                    _c.Send(Data);
                }
            }
            Team T;
            Kernel.Teams.TryRemove(TeamID, out T);
        }
    }
}