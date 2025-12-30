using Game.Core.Combat.Grid;
using Game.Core.Combat.Setup;
using Game.Core.Utils;

namespace Game.Core.Combat.Units
{
    public class UnitModelFactory
    {
        private uint nextId = 1;
        private readonly UnitModelRegistry modelRegistry;
        private readonly CellRegistry grid;

        public UnitModelFactory(UnitModelRegistry modelRegistry, CellRegistry grid)
        {
            this.modelRegistry = modelRegistry;
            this.grid = grid;
        }

        public Result<UnitModel, PlacementError> Spawn(Position2Int position, UnitTeam team, UnitData data)
        {
            if (data == null)
                return Result<UnitModel, PlacementError>.Failure(PlacementError.InvalidData);

            if (!grid.IsValid(position))
                return Result<UnitModel, PlacementError>.Failure(PlacementError.OutOfBounds);

            if (grid.GetCell(position).IsBlocked)
                return Result<UnitModel, PlacementError>.Failure(PlacementError.CellBlocked);

            if (modelRegistry.GetUnitAt(position) != null)
                return Result<UnitModel, PlacementError>.Failure(PlacementError.CellOccupied);

            var id = new UnitID(nextId++);
            var stats = new UnitStats(data.Strength, data.Dexterity, data.Constitution);
            var unit = new UnitModel(id, team, stats, position);

            modelRegistry.TryAdd(position, unit);
            grid.TrySetBlocked(position, true);

            return Result<UnitModel, PlacementError>.Success(unit);
        }
    }
}