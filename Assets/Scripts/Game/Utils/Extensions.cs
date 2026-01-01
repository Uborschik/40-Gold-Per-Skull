using UnityEngine;

namespace Game.Utils
{
    public static class Extensions
    {
        public static Vector2 ToCenter(this Vector2 v) =>
            new(v.x + 0.5f, v.y + 0.5f);

        public static Vector2 ToCenter(this Vector2Int v) =>
            new(v.x + 0.5f, v.y + 0.5f);
    }
}
