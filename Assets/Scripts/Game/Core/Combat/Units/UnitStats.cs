namespace Game.Core.Combat.Units
{
    public class UnitStats
    {
        public int Strength { get; }
        public int Dexterity { get; }
        public int Constitution { get; }

        public int DexterityModifier => CalculateModifier(Dexterity);
        public int StrengthModifier => CalculateModifier(Strength);
        public int ConstitutionModifier => CalculateModifier(Constitution);

        private static int CalculateModifier(int statValue) => (statValue - 10) / 2;

        public UnitStats(int strength, int dexterity, int constitution)
        {
            Strength = strength;
            Dexterity = dexterity;
            Constitution = constitution;
        }
    }
}