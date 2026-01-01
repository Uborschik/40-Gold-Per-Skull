namespace Game.Combat.TurnOrder
{
    public interface IRandomProvider
    {
        int D20();
        int Roll(int min, int max);
    }
}