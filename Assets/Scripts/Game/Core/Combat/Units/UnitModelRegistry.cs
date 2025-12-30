using Game.Core.Combat.Events;
using Game.Core.Combat.Grid;
using System.Collections.Generic;
using System.Linq;

namespace Game.Core.Combat.Units
{
    public class UnitModelRegistry
    {
        private readonly Dictionary<UnitID, UnitModel> byID = new();
        private readonly SpatialIndex spatialIndex;
        private readonly TeamRegistry teamRegistry;

        public UnitModelRegistry(CellRegistry cellRegistry)
        {
            spatialIndex = new SpatialIndex(cellRegistry);
            teamRegistry = new TeamRegistry();
        }

        // === Public API: Пространственные операции === //

        public bool TryAdd(Position2Int position, UnitModel unit)
        {
            if (unit == null) return false;
            if (byID.ContainsKey(unit.ID)) return false;
            if (!spatialIndex.TryAdd(unit.ID, position)) return false;

            byID[unit.ID] = unit;
            teamRegistry.Add(unit);
            unit.Position = position;
            return true;
        }

        public UnitModel GetUnitAt(Position2Int position) =>
            TryGetUnitByPosition(position, out var unit) ? unit : null;

        public bool TryRemoveAt(Position2Int position) =>
            TryGetUnitByPosition(position, out var unit) && TryRemoveUnit(unit);

        public bool TryMoveUnit(UnitID id, Position2Int to)
        {
            if (!byID.TryGetValue(id, out var unit)) return false;
            if (!spatialIndex.TryMove(id, to)) return false;

            unit.Position = to;
            return true;
        }

        // === Public API: Операции по ID === //

        public UnitModel GetUnitById(UnitID id) =>
            byID.TryGetValue(id, out var unit) ? unit : null;

        public IReadOnlyCollection<UnitModel> AllUnits => byID.Values;

        // === Public API: Операции по командам === //

        public IReadOnlyList<UnitModel> PlayerUnits => teamRegistry.GetTeam(UnitTeam.Player);
        public IReadOnlyList<UnitModel> EnemyUnits => teamRegistry.GetTeam(UnitTeam.Enemy);
        public bool HasAliveUnits(UnitTeam team) => teamRegistry.GetTeam(team).Any(u => u.IsAlive);

        public void RemoveDeadUnits(EventStream eventStream)
        {
            // Делаем снимок, чтобы безопасно итерировать
            var deadUnits = byID.Values.Where(u => !u.IsAlive).ToList();
            if (deadUnits.Count == 0) return;

            foreach (var unit in deadUnits)
            {
                // Удаляем из всех реестров атомарно
                spatialIndex.TryRemove(unit.ID);
                byID.Remove(unit.ID);
                teamRegistry.TryRemove(unit);

                eventStream?.Publish(new UnitDiedEvent(unit.ID, unit.Team));
            }
        }

        // === Private helpers === //

        private bool TryGetUnitByPosition(Position2Int position, out UnitModel unit)
        {
            unit = null;
            var unitId = spatialIndex.GetUnitAt(position);
            if (unitId.Value == 0) return false;
            return byID.TryGetValue(unitId, out unit);
        }

        private bool TryRemoveUnit(UnitModel unit)
        {
            if (unit == null) return false;

            spatialIndex.TryRemove(unit.ID);
            byID.Remove(unit.ID);
            teamRegistry.TryRemove(unit);
            return true;
        }
    }
}