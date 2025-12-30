using System;

namespace Game.Core.Combat.Grid
{
    public interface IGridSelector
    {
        event Action<Position2Int> Select;
    }
}