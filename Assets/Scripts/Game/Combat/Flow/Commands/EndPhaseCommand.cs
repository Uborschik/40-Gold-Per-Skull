namespace Game.Combat.Flow.Commands
{
    public class EndPhaseCommand : ICommand
    {
        private readonly CombatFlow flow;

        public EndPhaseCommand(CombatFlow flow)
        {
            this.flow = flow;
        }

        public void Execute() => flow.Advance();
    }
}