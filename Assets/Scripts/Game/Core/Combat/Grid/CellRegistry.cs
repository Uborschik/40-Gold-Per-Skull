using Game.Core.Combat.Setup;

namespace Game.Core.Combat.Grid
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

        public GridCell GetCell(Position2Int pos) =>
            IsValid(pos) ? grid[pos.X, pos.Y] : null;

        public bool TrySetBlocked(Position2Int pos, bool value)
        {
            var cell = GetCell(pos);
            if (cell == null) return false;
            cell.IsBlocked = value;
            return true;
        }

        public bool IsValid(Position2Int pos) =>
            pos.X >= 0 && pos.X < Width && pos.Y >= 0 && pos.Y < Height;
    }
}