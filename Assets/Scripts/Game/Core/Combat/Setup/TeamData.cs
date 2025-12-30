using Game.Core.Combat.Units;
using System;

namespace Game.Core.Combat.Setup
{
    [Serializable]
    public class TeamData
    {
        public UnitTeam Team;
        public UnitData[] Units;
    }
}