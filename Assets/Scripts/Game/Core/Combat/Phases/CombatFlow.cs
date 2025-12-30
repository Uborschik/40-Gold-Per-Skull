using Game.Core.Combat.Events;
using Game.Core.Combat.Grid;

namespace Game.Core.Combat.Phases
{
    public class CombatFlow
    {
        private readonly EventStream eventStream;
        private readonly IGridSelector gridSelector;
        private readonly ICombatPhase[] phases;
        private int currentIndex;

        public CombatFlow(EventStream eventStream, IGridSelector gridSelector, PlacementPhase placement, CombatPhase combat)
        {
            this.eventStream = eventStream;
            this.gridSelector = gridSelector;
            phases = new ICombatPhase[] { placement, combat };
            currentIndex = 0;

            gridSelector.Select += OnSelected;
        }

        private void OnSelected(Position2Int position)
        {
            var phase = phases[currentIndex];

            if (phase.SelectedUnit == null)
                phase.TrySelect(position);
            else phase.TryPlaceSelected(position);
        }

        public void Start()
        {
            while (currentIndex < phases.Length && phases[currentIndex].IsComplete)
            {
                currentIndex++;
            }
            if (currentIndex < phases.Length)
            {
                var phase = phases[currentIndex];
                phase.Enter();
                eventStream.Publish(new PhaseChangedEvent(phase));
            }
        }

        public void Tick()
        {
            if (currentIndex >= phases.Length) return;

            var phase = phases[currentIndex];

            if (phase.IsComplete)
            {
                TransitionToNextPhase();
                return;
            }

            phase.Update();
        }

        private void TransitionToNextPhase()
        {
            if (currentIndex < phases.Length)
            {
                phases[currentIndex].Exit();
            }

            currentIndex++;
            while (currentIndex < phases.Length && phases[currentIndex].IsComplete)
            {
                currentIndex++;
            }

            if (currentIndex < phases.Length)
            {
                var phase = phases[currentIndex];
                phase.Enter();
                eventStream.Publish(new PhaseChangedEvent(phase));
            }
        }
    }
}