using Game.Core.Combat.Grid;
using System;
using UnityEngine;

[Serializable]
public struct SerializablePlacementArea
{
    public BoundsInt Area;

    public SerializablePlacementArea(Vector3Int min, Vector3Int max) : this()
    {
        Area = new(min, max);
    }

    public readonly RectangleArea ToCore() => new(
        new Position2Int(Area.min.x, Area.min.y),
        new Position2Int(Area.max.x, Area.max.y)
    );
}