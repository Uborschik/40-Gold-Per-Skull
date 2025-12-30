public class GridCell
{
    public int X { get; }
    public int Y { get; }
    public float MovementCostMultiplier { get; set; } = 1f;
    public bool IsBlocked { get; set; } = false;
    public bool IsWalkable => MovementCostMultiplier <= 0 || !IsBlocked;

    public GridCell(int x, int y)
    {
        X = x;
        Y = y;
    }
}