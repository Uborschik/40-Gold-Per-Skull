using Game.Combat.Application.Notifications;
using Game.Combat.Core.Entities;
using Game.Combat.Entities.Units;
using Game.Combat.Flow.Phases;
using Game.Combat.Infrastructure.Input;
using Game.Combat.Infrastructure.TurnOrder;
using Game.Combat.Infrastructure.View;
using Game.Utils;
using UnityEngine;

namespace Game.Combat.Infrastructure.Systems
{
    public class TurnSystem : INotifyUnitMoved, INotifyUnitDied, INotifyPhaseChanged, INotifyTurnChanged
    {
        private readonly TurnQueue turnQueue;
        private readonly UnitRegistry unitRegistry;
        private readonly GridView gridView;

        public TurnSystem(
            TurnQueue turnQueue,
            UnitRegistry unitRegistry,
            GridView gridView)
        {
            this.turnQueue = turnQueue;
            this.unitRegistry = unitRegistry;
            this.gridView = gridView;
        }

        void INotifyPhaseChanged.PhaseEntered(ICombatPhase phase)
        {
            if (phase is CombatPhase)
            {
                StartNewRound();
            }
        }

        void INotifyPhaseChanged.PhaseExited(ICombatPhase phase) { }

        void INotifyUnitMoved.UnitMoved(Unit unit, Vector2Int newPosition)
        {
            unit.SetOrder(unitRegistry.Height - newPosition.y);
        }

        void INotifyUnitDied.UnitDied(Unit unit)
        {
            turnQueue.Remove(unit);

            var winner = CheckVictory();
            if (winner.HasValue)
            {
                Victory(winner.Value);
            }
        }

        private void Victory(Team winner)
        {
            Debug.Log($"[TurnSystem] Victory: {winner}");
        }

        void INotifyUnitDied.Victory(Team winner)
        {
            Debug.Log($"[TurnSystem] Victory: {winner}");
        }

        void INotifyTurnChanged.TurnChanged(Unit currentUnit)
        {
            if (currentUnit != null)
            {
                HighlightCurrentUnit(currentUnit);
            }
        }

        private void StartNewRound()
        {
            var aliveUnits = unitRegistry.GetAllAliveUnits();
            turnQueue.Build(aliveUnits);
            HighlightCurrentUnit(turnQueue.Current);
        }

        private void HighlightCurrentUnit(Unit unit)
        {
            if (unit == null) return;

            var type = unit.Team == Team.Player ? SelectionType.AllyUnit : SelectionType.EnemyUnit;
            gridView.PaintTurnCellSelection(unit.Position.ToCenter(), type);
        }

        private Team? CheckVictory()
        {
            var playerUnits = unitRegistry.GetTeam(Team.Player);
            var enemyUnits = unitRegistry.GetTeam(Team.Enemy);

            if (playerUnits.Count == 0) return Team.Enemy;
            if (enemyUnits.Count == 0) return Team.Player;

            return null;
        }
    }
}