using UnityEngine;

namespace Game.Combat.Entities.Grid
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

        public bool TrySetBlocked(Vector2Int oldPos, Vector2Int newPos)
        {
            var oldCell = GetCell(oldPos);
            var newCell = GetCell(newPos);
            if (oldCell == null || newCell == null) return false;
            oldCell.IsBlocked = false;
            newCell.IsBlocked = true;
            return true;
        }

        public bool IsValid(Vector2Int pos) =>
            pos.x >= 0 && pos.x < Width && pos.y >= 0 && pos.y < Height;

        public bool IsWalkable(Vector2Int target)
        {
            var cell = GetCell(target);

            if (cell == null) return false;

            return cell.IsWalkable;
        }
    }
}