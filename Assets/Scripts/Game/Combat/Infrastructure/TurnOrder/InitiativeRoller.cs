using Game.Combat.Entities.Units;

namespace Game.Combat.Infrastructure.TurnOrder
{
    public class InitiativeRoller
    {
        private readonly IRandomProvider random;
        public InitiativeRoller(IRandomProvider random) => this.random = random;

        public int Roll(Unit unit) => unit.Stats.Dexterity;
    }
}