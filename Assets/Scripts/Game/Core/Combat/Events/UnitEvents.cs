using Game.Core.Combat.Grid;
using Game.Core.Combat.Units;

namespace Game.Core.Combat.Events
{
    public readonly struct UnitDiedEvent : ICombatEvent
    {
        public UnitID UnitId { get; }
        public UnitTeam Team { get; }
        public UnitDiedEvent(UnitID unitId, UnitTeam team) => (UnitId, Team) = (unitId, team);
    }

    public readonly struct UnitMovedEvent : ICombatEvent
    {
        public UnitID UnitId { get; }
        public Position2Int From { get; }
        public Position2Int To { get; }
        public UnitMovedEvent(UnitID unitId, Position2Int from, Position2Int to) =>
            (UnitId, From, To) = (unitId, from, to);
    }

    public readonly struct UnitSelectedEvent : ICombatEvent
    {
        public UnitID UnitId { get; }
        public Position2Int? Position { get; }
        public UnitSelectedEvent(UnitID unitId, Position2Int? position)
        {
            UnitId = unitId; Position = position;
        }
    }
}
