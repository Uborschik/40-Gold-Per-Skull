using Game.Combat.Entities.Units;
using Game.Combat.Flow.Phases;
using UnityEngine;

namespace Game.Combat.Application.Events
{
    public record UnitSelected(Unit Unit);
    public record UnitDeselected;
    public record UnitMoved(Unit Unit, Vector2Int NewPosition);
    public record TurnChanged(Unit CurrentUnit);
    public record PhaseChanged(ICombatPhase Phase, bool IsEntering);
}