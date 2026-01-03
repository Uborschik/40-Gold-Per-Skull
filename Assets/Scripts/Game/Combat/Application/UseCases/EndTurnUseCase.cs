using Game.Combat.Application.Notifications;
using Game.Combat.Infrastructure.TurnOrder;

namespace Game.Combat.Application.UseCases
{
    public class EndTurnUseCase
    {
        private readonly TurnQueue turnQueue;
        private readonly INotifyUnitSelected[] selectionListeners;
        private readonly INotifyTurnChanged[] turnChangedListeners;

        public EndTurnUseCase(
            TurnQueue turnQueue,
            INotifyUnitSelected[] selectionListeners,
            INotifyTurnChanged[] turnChangedListeners)
        {
            this.turnQueue = turnQueue;
            this.selectionListeners = selectionListeners;
            this.turnChangedListeners = turnChangedListeners;
        }

        public void Execute()
        {
            turnQueue.Advance();

            foreach (var listener in selectionListeners)
                listener.UnitDeselected();

            var currentUnit = turnQueue.Current;
            if (currentUnit != null)
                foreach (var listener in turnChangedListeners)
                    listener?.TurnChanged(currentUnit);
        }
    }
}