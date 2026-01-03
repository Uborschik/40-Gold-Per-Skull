using Game.Combat.Application.Notifications;
using Game.Combat.Entities.Grid;
using Game.Combat.Entities.Units;
using Game.Utils;
using UnityEngine;

namespace Game.Combat.Application.UseCases
{
    public class MoveUnitUseCase
    {
        private readonly CellRegistry cellRegistry;
        private readonly UnitRegistry unitRegistry;
        private readonly INotifyUnitMoved[] listeners;

        public MoveUnitUseCase(
            CellRegistry cellRegistry,
            UnitRegistry unitRegistry,
            INotifyUnitMoved[] listeners)
        {
            this.cellRegistry = cellRegistry;
            this.unitRegistry = unitRegistry;
            this.listeners = listeners;
        }

        public bool Execute(Unit unit, Vector2Int target)
        {
            if (unit == null || !cellRegistry.IsWalkable(target))
                return false;

            var oldPosition = unit.Position.ToInt();

            if (unitRegistry.TryMoveUnit(unit, target))
            {
                cellRegistry.TrySetBlocked(oldPosition, false);
                cellRegistry.TrySetBlocked(target, true);
                unit.SetPosition(target.ToCenter());

                foreach (var listener in listeners)
                    listener.UnitMoved(unit, target);

                return true;
            }
            return false;
        }
    }
}