using Game.Core.Combat.Setup;
using UnityEngine;

namespace Game.Combat.Grid
{
    public class CellRegistry
    {
        private readonly GridCell[,] grid;
        public int Width => grid.GetLength(0);
        public int Height => grid.GetLength(1);

        public CellRegistry(GridData data)
        {
            grid = new GridCell[data.GridWidth, data.GridHeight];
            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                    grid[x, y] = new GridCell(x, y);
        }

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