using Game.Combat.Entities.Units;
using System.Collections.Generic;
using System.Linq;

namespace Game.Combat.TurnOrder
{
    public class TurnQueue
    {
        // Simplified: cyclic queue. TODO: Per-round initiative if needed
        private readonly Queue<Unit> order = new();
        private readonly IRandomProvider random;

        public TurnQueue(IRandomProvider random)
        {
            this.random = random;
        }

        public void Build(IEnumerable<Unit> units)
        {
            var roller = new InitiativeRoller(random);
            order.Clear();

            foreach (var unit in units.OrderByDescending(u => roller.Roll(u)))
                order.Enqueue(unit);
        }

        public Unit Current => order.Count > 0 ? order.Peek() : null;

        public void Advance()
        {
            if (order.Count == 0) return;

            var completedUnit = order.Dequeue();
            order.Enqueue(completedUnit);
        }

        public bool IsRoundComplete() =>
            order.Count == 0;
    }
}