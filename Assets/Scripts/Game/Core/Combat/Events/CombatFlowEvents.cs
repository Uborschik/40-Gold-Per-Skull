using Game.Core.Combat.Phases;
using Game.Core.Combat.Units;

namespace Game.Core.Combat.Events
{
    public readonly struct CombatEndedEvent : ICombatEvent
    {
        public UnitTeam Winner { get; }
        public CombatEndedEvent(UnitTeam winner) => Winner = winner;
    }

    public readonly struct PhaseChangedEvent : ICombatEvent
    {
        public ICombatPhase NewPhase { get; }
        public PhaseChangedEvent(ICombatPhase newPhase) => NewPhase = newPhase;
    }

    public readonly struct TurnCompletedEvent : ICombatEvent
    {
        public UnitID UnitId { get; }
        public TurnCompletedEvent(UnitID unitId) => UnitId = unitId;
    }
}