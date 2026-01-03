using Game.Combat.Core.Entities;
using System;

namespace Game.Combat.Setup
{
    [Serializable]
    public class TeamData
    {
        public Team Team;
        public UnitData[] Units;
    }
}