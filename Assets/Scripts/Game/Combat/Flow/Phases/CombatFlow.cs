namespace Game.Combat.Phases
{
    public class CombatFlow
    {
        private readonly ICombatPhase[] phases;
        private int currentIndex;

        public CombatFlow(PlacementPhase placement, CombatPhase combat)
        {
            phases = new ICombatPhase[] { placement, combat };
            currentIndex = 0;
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
            }
        }
    }
}