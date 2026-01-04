// Game/Combat/Application/UseCases/MoveUnitUseCase.cs
using Game.Combat.Application.Events;
using Game.Combat.Entities.Grid;
using Game.Combat.Entities.Units;
using Game.Utils;
using UnityEngine;

namespace Game.Combat.Application.UseCases
{
    public class MoveUnitUseCase
    {
        private readonly CellRegistry cells;
        private readonly UnitRegistry units;
        private readonly IEventBus events;

        public MoveUnitUseCase(CellRegistry cells, UnitRegistry units, IEventBus events)
        {
            this.cells = cells;
            this.units = units;
            this.events = events;
        }

        public bool Execute(Unit unit, Vector2Int target)
        {
            if (!cells.IsWalkable(target)) return false;

            var oldPos = unit.Position.ToInt();
            if (!units.TryMoveUnit(unit, target)) return false;

            cells.TrySetBlocked(oldPos, target);
            unit.SetPosition(target.ToCenter());

            events.Publish(new UnitMoved(unit, target));
            return true;
        }
    }
}