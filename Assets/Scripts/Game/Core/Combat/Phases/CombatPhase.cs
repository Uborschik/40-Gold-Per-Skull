using Game.Core.Combat.Events;
using Game.Core.Combat.Grid;
using Game.Core.Combat.TurnOrder;
using Game.Core.Combat.Units;
using System.Linq;

namespace Game.Core.Combat.Phases
{
    public class CombatPhase : ICombatPhase
    {
        private readonly EventStream eventStream;
        private readonly UnitModelRegistry modelRegistry;
        private readonly TurnQueue turnQueue;

        public bool IsComplete { get; private set; }

        public UnitModel SelectedUnit => throw new System.NotImplementedException();

        public CombatPhase(EventStream eventStream, UnitModelRegistry modelRegistry, TurnQueue turnQueue)
        {
            this.eventStream = eventStream;
            this.modelRegistry = modelRegistry;
            this.turnQueue = turnQueue;
        }

        public void Enter()
        {
            eventStream.Subscribe<TurnCompletedEvent>(OnTurnCompleted);
            eventStream.Subscribe<UnitDiedEvent>(OnUnitDied);

            StartNewRound();
        }

        public void Update() { /* Ожидание завершения хода */ }

        public void Exit()
        {
            eventStream.Unsubscribe<TurnCompletedEvent>(OnTurnCompleted);
        }

        private void StartNewRound()
        {
            modelRegistry.RemoveDeadUnits(eventStream);

            var aliveUnits = modelRegistry.AllUnits.Where(u => u.IsAlive);
            turnQueue.Build(aliveUnits);
        }

        private void OnTurnCompleted(TurnCompletedEvent e)
        {
            if (turnQueue.IsRoundComplete())
            {
                StartNewRound();
            }
        }

        private void OnUnitDied(UnitDiedEvent e)
        {
            var winner = CheckVictoryTeam();

            if (winner.HasValue)
            {
                IsComplete = true;
                eventStream.Publish(new CombatEndedEvent(winner.Value));
            }
        }

        private UnitTeam? CheckVictoryTeam()
        {
            if (!modelRegistry.HasAliveUnits(UnitTeam.Player)) return UnitTeam.Enemy;
            if (!modelRegistry.HasAliveUnits(UnitTeam.Enemy)) return UnitTeam.Player;

            return null;
        }

        public bool TrySelect(Position2Int position)
        {
            throw new System.NotImplementedException();
        }

        public bool TryPlaceSelected(Position2Int newPosition)
        {
            throw new System.NotImplementedException();
        }

        public void ConfirmPlacement()
        {
            throw new System.NotImplementedException();
        }
    }
}