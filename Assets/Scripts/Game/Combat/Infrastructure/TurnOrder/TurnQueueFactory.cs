using Game.Combat.Entities.Units;
using System.Collections.Generic;
using System.Linq;

namespace Game.Combat.Infrastructure.TurnOrder
{
    public class TurnQueueFactory
    {
        private readonly IRandomProvider random;

        public TurnQueueFactory(IRandomProvider random)
        {
            this.random = random;
        }

        public List<Unit> Create(List<Unit> units)
        {
            var order = new List<Unit>();

            var roller = new InitiativeRoller(random);
            order.Clear();

            foreach (var unit in units.OrderByDescending(u => roller.Roll(u)))
                order.Add(unit);

            return order;
        }
    }
}