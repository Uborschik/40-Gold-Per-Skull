using System;
using System.Collections.Generic;

namespace Game.Core.Combat.Grid
{
    [Serializable]
    public struct RectangleArea : IArea
    {
        public Position2Int Min;
        public Position2Int Max;

        public RectangleArea(Position2Int min, Position2Int max)
        {
            Min = min;
            Max = max;
        }

        public readonly bool Contains(Position2Int position) =>
            position.X >= Min.X && position.X < Max.X &&
            position.Y >= Min.Y && position.Y < Max.Y;

        public readonly IEnumerable<Position2Int> AllCells()
        {
            for (int y = Min.Y; y < Max.Y; y++)
                for (int x = Min.X; x < Max.X; x++)
                    yield return new Position2Int(x, y);
        }
    }
}