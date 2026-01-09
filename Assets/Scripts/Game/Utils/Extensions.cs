using System.Collections.Generic;
using UnityEngine;

namespace Game.Utils
{
    public static class Extensions
    {
        public static Vector2Int ToInt(this Vector2 v) =>
            new(Mathf.FloorToInt(v.x), Mathf.FloorToInt(v.y));

        public static Vector2 ToCenter(this Vector2 v) =>
            new(Mathf.Floor(v.x) + 0.5f, Mathf.Floor(v.y) + 0.5f);

        public static Vector2 ToCenter(this Vector2Int v) =>
            new(v.x + 0.5f, v.y + 0.5f);

        public static Vector3Int To3Int(this Vector2Int v) =>
            new(v.x, v.y);

        public static IEnumerable<Vector3Int> AllCells<T>(this T[,] array)
        {
            for (int y = 0; y < array.GetLength(1); y++)
                for (int x = 0; x < array.GetLength(0); x++)
                    yield return new Vector3Int(x, y);
        }

        public static IEnumerable<Vector3Int> AllCells(this BoundsInt area)
        {
            for (int y = area.min.y; y < area.max.y; y++)
                for (int x = area.min.x; x < area.max.x; x++)
                    yield return new Vector3Int(x, y);
        }
    }
}
