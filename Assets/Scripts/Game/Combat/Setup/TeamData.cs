using Game.Combat.Entities.Units;
using System;

namespace Game.Core.Combat.Setup
{
    [Serializable]
    public class TeamData
    {
        public Team Team;
        public UnitData[] Units;
    }
}