using Game.Core.Combat.Units;
using System.Collections.Generic;

namespace Game.Core.Combat.Grid
{
    internal class SpatialIndex
    {
        private readonly Dictionary<Position2Int, UnitID> positionToId = new();
        private readonly Dictionary<UnitID, Position2Int> idToPosition = new();
        private readonly CellRegistry cellRegistry;

        public SpatialIndex(CellRegistry cellRegistry)
        {
            this.cellRegistry = cellRegistry;
        }

        public bool TryAdd(UnitID id, Position2Int position)
        {
            if (!cellRegistry.IsValid(position)) return false;
            if (positionToId.ContainsKey(position)) return false;

            positionToId[position] = id;
            idToPosition[id] = position;
            return true;
        }

        public bool TryRemove(UnitID id)
        {
            if (!idToPosition.TryGetValue(id, out var position)) return false;

            idToPosition.Remove(id);
            positionToId.Remove(position);
            return true;
        }

        public bool TryMove(UnitID id, Position2Int to)
        {
            if (!cellRegistry.IsValid(to)) return false;
            if (positionToId.ContainsKey(to)) return false;
            if (!idToPosition.ContainsKey(id)) return false;

            var from = idToPosition[id];
            cellRegistry.TrySetBlocked(from, false);
            positionToId.Remove(from);
            positionToId[to] = id;
            idToPosition[id] = to;
            cellRegistry.TrySetBlocked(to, true);
            return true;
        }

        public UnitID GetUnitAt(Position2Int position) =>
            positionToId.TryGetValue(position, out var id) ? id : default;

        public bool IsOccupied(Position2Int position) => positionToId.ContainsKey(position);
    }
}