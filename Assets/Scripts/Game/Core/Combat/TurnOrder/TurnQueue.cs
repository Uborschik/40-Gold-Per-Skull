using Game.Core.Combat.Events;
using Game.Core.Combat.Units;
using System.Collections.Generic;
using System.Linq;

namespace Game.Core.Combat.TurnOrder
{
    public class TurnQueue
    {
        // Simplified: cyclic queue. TODO: Per-round initiative if needed
        private readonly Queue<UnitModel> order = new();
        private readonly EventStream eventStream;
        private readonly IRandomProvider random;
        private readonly HashSet<UnitID> roundParticipants = new();
        public TurnQueue(EventStream eventStream, IRandomProvider random)
        {
            this.eventStream = eventStream;
            this.random = random;
        }

        public void Build(IEnumerable<UnitModel> units)
        {
            var roller = new InitiativeRoller(random);
            order.Clear();
            roundParticipants.Clear();

            foreach (var unit in units.OrderByDescending(u => roller.Roll(u)))
                order.Enqueue(unit);
        }

        public UnitModel Current => order.Count > 0 ? order.Peek() : null;

        public void Advance()
        {
            if (order.Count == 0) return;

            var completedUnit = order.Dequeue();
            roundParticipants.Add(completedUnit.ID);
            order.Enqueue(completedUnit);

            eventStream.Publish(new TurnCompletedEvent(completedUnit.ID));
        }

        public bool IsRoundComplete() =>
            order.Count == 0 || roundParticipants.Count == order.Count;
    }
}