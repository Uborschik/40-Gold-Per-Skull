using Game.Combat.Application.Events;
using Game.Combat.Infrastructure.TurnOrder;

namespace Game.Combat.Application.UseCases
{
    public class EndTurnUseCase
    {
        private readonly TurnQueue turnQueue;
        private readonly IEventBus events;

        public EndTurnUseCase(TurnQueue turnQueue, IEventBus events)
        {
            this.turnQueue = turnQueue;
            this.events = events;
        }

        public void Execute()
        {
            turnQueue.Advance();
            events.Publish(new TurnChanged(turnQueue.Current));
        }
    }
}