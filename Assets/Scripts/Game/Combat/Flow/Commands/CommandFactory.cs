using VContainer;

namespace Game.Combat.Flow.Commands
{
    public class CommandFactory : ICommandFactory
    {
        private readonly IObjectResolver resolver;
        public CommandFactory(IObjectResolver resolver) => this.resolver = resolver;

        public ICommand CreateEndPhaseCommand() => resolver.Resolve<EndPhaseCommand>();
        public ICommand CreateEndTurnCommand() => resolver.Resolve<EndTurnCommand>();
    }
}