using Game.Combat.Entities.Units;
using System.Collections.Generic;
using System.Linq;

namespace Game.Combat.Infrastructure.TurnOrder
{
    public class TurnQueue
    {
        private readonly Queue<Unit> order = new();
        private readonly IRandomProvider random;

        private Unit firstInRound;

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

            firstInRound = order.Count > 0 ? order.Peek() : null;
        }

        public Unit Current => order.Count > 0 ? order.Peek() : null;

        public void Advance()
        {
            if (order.Count == 0) return;

            var completedUnit = order.Dequeue();
            order.Enqueue(completedUnit);
        }

        public void Remove(Unit unit)
        {
            var temp = order.Where(u => u != unit).ToList();
            order.Clear();
            foreach (var u in temp) order.Enqueue(u);
        }

        public bool IsRoundComplete() =>
            order.Count == 0 || (order.Count > 0 && order.Peek() == firstInRound);
    }
}