using Game.Combat.TurnOrder;

namespace Game.Core.Combat.TurnOrder
{
    public class SystemRandomProvider : IRandomProvider
    {
        private readonly System.Random rnd = new();
        public int D20() => rnd.Next(1, 21);
        public int Roll(int min, int max) => rnd.Next(min, max + 1);
    }
}