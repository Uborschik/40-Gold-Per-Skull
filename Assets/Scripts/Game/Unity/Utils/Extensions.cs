using Game.Core.Combat.Grid;
using UnityEngine;

namespace Game.Unity.Utils
{
    public static class Extensions
    {
        private const float EPSILON = 0.001f;

        public static Vector3Int ToVector3Int(this Position2Int position) =>
            new(position.X, position.Y);

        public static Vector2 ToCenter2(this Position2Int position) =>
            new(position.X + 0.5f, position.Y + 0.5f);

        public static Position2Int ToPosition(this Vector3 worldPos, CellRegistry grid = null)
        {
            var pos = new Position2Int(
                Mathf.FloorToInt(worldPos.x + EPSILON),
                Mathf.FloorToInt(worldPos.y + EPSILON)
            );

            if (grid != null && !grid.IsValid(pos))
            {
                Debug.LogWarning($"Position {pos} is out of grid bounds!");
            }

            return pos;
        }

        public static Position2Int? ToSafeGridPosition(this Vector3 worldPos, CellRegistry grid)
        {
            if (grid == null) return null;

            var pos = worldPos.ToPosition(grid);
            return grid.IsValid(pos) ? pos : null;
        }
    }
}