namespace Game.Combat.Flow.Commands
{
    public class EndPhaseCommand : ICommand
    {
        private readonly CombatFlow combatFlow;

        public EndPhaseCommand(CombatFlow combatFlow)
        {
            this.combatFlow = combatFlow;
        }

        public void Execute() => combatFlow.AdvancePhase();
    }
}