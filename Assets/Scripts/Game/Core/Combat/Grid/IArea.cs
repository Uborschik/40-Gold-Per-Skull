using System.Collections.Generic;

namespace Game.Core.Combat.Grid
{
    public interface IArea
    {
        bool Contains(Position2Int position);
        IEnumerable<Position2Int> AllCells();
    }
}