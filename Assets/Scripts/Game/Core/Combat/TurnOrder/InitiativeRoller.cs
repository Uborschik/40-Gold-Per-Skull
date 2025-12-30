using Game.Core.Combat.Units;

namespace Game.Core.Combat.TurnOrder
{
    public class InitiativeRoller
    {
        private readonly IRandomProvider random;
        public InitiativeRoller(IRandomProvider random) => this.random = random;

        public int Roll(UnitModel unit) => random.D20() + unit.Stats.DexterityModifier;
    }
}