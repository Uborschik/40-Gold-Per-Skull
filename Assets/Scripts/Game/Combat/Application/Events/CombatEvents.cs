using Game.Combat.Application.Turns;
using Game.Combat.Core.Entities;
using Game.Combat.Entities.Units;
using Game.Combat.Flow;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Combat.Application.Events
{
    public record UnitSelected(Unit Unit);
    public record UnitDeselected;
    public record UnitMoved(Unit Unit, Vector2Int NewPosition);
    public record UnitAttacked(Unit Attacker, Unit Target, int Damage);
    public record UnitActionPointsChanged(Unit Unit, int RemainingActions);
    public record UnitDied(Unit Unit);
    public record UnitMoveStarted(Unit Unit, Vector2Int Target);
    public record UnitAttackStarted(Unit Attacker, Unit Target);

    public record TurnStarted(Unit Unit, TurnType Type);
    public record TurnEnded(Unit Unit, TurnType Type);
    public record PhaseChanged(Phase Phase);
    public record BattleEnded(Team? Team);
    public record ShowMoveableAreaRequested(HashSet<Vector2Int> Positions);
    public record HideMoveableAreaRequested();
    public record ShowAttackRangeRequested(HashSet<Vector2Int> Positions);
    public record HideAttackRangeRequested();
    public record ActionValidationFailed(string Reason);
    public record InvalidActionAttempted(Vector2Int Position);
    public record TurnStateCompleted(Unit Unit);
}