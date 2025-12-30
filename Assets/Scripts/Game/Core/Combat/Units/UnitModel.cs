using Game.Core.Combat.Grid;

namespace Game.Core.Combat.Units
{
    public class UnitModel
    {
        public UnitID ID { get; }
        public UnitTeam Team { get; }
        public UnitStats Stats { get; }
        public Position2Int Position { get; internal set; }
        public bool IsAlive { get; internal set; } = true;

        public UnitModel(UnitID id, UnitTeam team, UnitStats stats, Position2Int position)
        {
            ID = id;
            Team = team;
            Stats = stats;
            Position = position;
        }
    }
}