using Game.Combat.Entities.Grid;
using UnityEngine;

namespace Game.Combat.Grid
{
    public class CellRegistry
    {
        private GridCell[,] grid;
        public GridCell[,] Grid => grid;
        public int Width => grid.GetLength(0);
        public int Height => grid.GetLength(1);

        public void AddGrid(GridCell[,] grid) => this.grid = grid;

        public GridCell GetCell(Vector2Int pos) =>
            IsValid(pos) ? grid[pos.x, pos.y] : null;

        public bool TrySetBlocked(Vector2Int pos, bool value)
        {
            var cell = GetCell(pos);
            if (cell == null) return false;
            cell.IsBlocked = value;
            return true;
        }

        public bool IsValid(Vector2Int pos) =>
            pos.x >= 0 && pos.x < Width && pos.y >= 0 && pos.y < Height;
    }
}