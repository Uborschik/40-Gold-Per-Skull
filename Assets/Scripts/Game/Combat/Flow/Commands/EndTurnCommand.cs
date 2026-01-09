using Game.Combat.Application.Turns;

namespace Game.Combat.Flow.Commands
{
    public class EndTurnCommand : ICommand
    {
        private readonly BattleCyclic battleCyclic;

        public EndTurnCommand(BattleCyclic battleCyclic)
        {
            this.battleCyclic = battleCyclic;
        }

        public void Execute() => battleCyclic.RequestEndPlayerTurn();
    }
}