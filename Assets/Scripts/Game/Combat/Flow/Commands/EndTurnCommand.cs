using Game.Combat.Flow.Phases;

namespace Game.Combat.Flow.Commands
{
    public class EndTurnCommand : ICommand
    {
        private CombatPhase combatPhase;

        public EndTurnCommand(CombatPhase combatPhase)
        {
            this.combatPhase = combatPhase;
        }

        public void Execute() => combatPhase.EndTurn();
    }
}