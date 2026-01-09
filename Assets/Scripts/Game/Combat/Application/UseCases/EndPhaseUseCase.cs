using Game.Combat.Application.Events;
using Game.Combat.Flow;

namespace Game.Combat.Application.UseCases
{
    public class EndPhaseUseCase
    {
        private readonly CombatFlow flow;
        private readonly IEventBus events;

        public EndPhaseUseCase(CombatFlow flow, IEventBus events)
        {
            this.flow = flow;
            this.events = events;
        }

        public void Execute()
        {
            flow.Advance();
            events.Publish(new PhaseChanged(flow.CurrentPhase));
        }
    }
}
