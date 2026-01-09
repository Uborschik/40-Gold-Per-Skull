using Game.Combat.Application.Events;
using Game.Combat.Entities.Grid;
using Game.Combat.Entities.Units;
using Game.Utils;
using UnityEngine;

namespace Game.Combat.Application.UseCases
{
    public class MoveUnitUseCase
    {
        private readonly IEventBus events;
        private readonly CellRegistry cells;
        private readonly UnitRegistry units;

        public MoveUnitUseCase(IEventBus events, CellRegistry cells, UnitRegistry units)
        {
            this.events = events;
            this.cells = cells;
            this.units = units;
        }

        public bool Execute(Unit unit, Vector2Int target)
        {
            if (!cells.IsWalkable(target)) return false;
            if (!units.TryMoveUnit(unit, target)) return false;

            cells.TrySetBlocked(unit.Position.ToInt(), target);
            unit.SetPosition(target.ToCenter());
            unit.SetOrder(cells.Height - target.y);

            events.Publish(new UnitMoved(unit, target));
            return true;
        }
    }
}