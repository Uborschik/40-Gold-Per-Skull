namespace Game.Combat.Infrastructure.TurnOrder
{
    public interface IRandomProvider
    {
        int D20();
        int Roll(int min, int max);
    }
}