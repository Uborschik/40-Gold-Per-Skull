using UnityEngine;

namespace Game.Combat.Entities.Grid
{
    public class GridCell
    {
        public Vector2Int Position { get; }
        public float MovementCostMultiplier { get; set; } = 1f;
        public bool IsBlocked { get; set; } = false;
        public bool IsWalkable => MovementCostMultiplier <= 0 || !IsBlocked;

        public GridCell(int x, int y)
        {
            Position = new Vector2Int(x, y);
        }
    }
}