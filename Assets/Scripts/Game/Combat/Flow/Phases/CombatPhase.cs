using Game.Combat.Entities.Selector;
using Game.Combat.Entities.Units;
using Game.Combat.TurnOrder;
using Game.Combat.Units;
using UnityEngine;

namespace Game.Combat.Phases
{
    public class CombatPhase : ICombatPhase
    {
        private readonly UnitRegistry unitRegistry;
        private readonly TurnQueue turnQueue;

        public bool IsComplete { get; private set; }

        public CombatPhase(UnitRegistry unitRegistry, TurnQueue turnQueue)
        {
            this.unitRegistry = unitRegistry;
            this.turnQueue = turnQueue;
        }

        public void Enter()
        {
            StartNewRound();
        }

        public void Update() {}

        public void Exit()
        {
        }

        private void StartNewRound()
        {
            var aliveUnits = unitRegistry.GetAllAliveUnits();
            turnQueue.Build(aliveUnits);
        }

        private void OnTurnCompleted()
        {
            if (turnQueue.IsRoundComplete())
            {
                StartNewRound();
            }
        }

        private void OnUnitDied()
        {
            var winner = CheckVictoryTeam();

            if (winner.HasValue)
            {
                IsComplete = true;
            }
        }

        private Team? CheckVictoryTeam()
        {
            if (unitRegistry.GetTeam(Team.Player).Count == 0) return Team.Enemy;
            if (unitRegistry.GetTeam(Team.Enemy).Count == 0) return Team.Player;

            return null;
        }

        public void UpdateHover(Vector2Int position, HighlightType type)
        {
            throw new System.NotImplementedException();
        }

        public void UpdateClick(Vector2Int position, SelectionType type)
        {
            throw new System.NotImplementedException();
        }
    }
}