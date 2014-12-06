using System;
using System.Collections.Generic;

namespace GameServer.Interface
{
    public interface Event
    {
        Dictionary<DayOfWeek, List<Time>> Schedule { get; set; }
        void Start();
    }
}
