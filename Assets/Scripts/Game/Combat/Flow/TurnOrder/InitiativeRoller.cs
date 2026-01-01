using Game.Combat.Entities.Units;

namespace Game.Combat.TurnOrder
{
    public class InitiativeRoller
    {
        private readonly IRandomProvider random;
        public InitiativeRoller(IRandomProvider random) => this.random = random;

        public int Roll(Unit unit) => random.D20() + unit.Stats.DexterityModifier;
    }
}