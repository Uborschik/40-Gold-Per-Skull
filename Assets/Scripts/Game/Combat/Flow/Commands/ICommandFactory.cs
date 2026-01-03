namespace Game.Combat.Flow.Commands
{
    public interface ICommandFactory
    {
        ICommand CreateEndPhaseCommand();
        ICommand CreateEndTurnCommand();
    }
}