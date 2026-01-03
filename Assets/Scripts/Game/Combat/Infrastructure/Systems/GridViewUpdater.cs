using Game.Combat.Application.Notifications;
using Game.Combat.Core.Entities;
using Game.Combat.Entities.Grid;
using Game.Combat.Entities.Units;
using Game.Combat.Infrastructure.Input;
using Game.Combat.Infrastructure.View;
using Game.Utils;
using UnityEngine;

namespace Game.Combat.Infrastructure.Systems
{
    public class GridViewUpdater : INotifyUnitMoved, INotifyUnitSelected
    {
        private readonly GridView gridView;
        private readonly CellRegistry cellRegistry;

        public GridViewUpdater(GridView gridView, CellRegistry cellRegistry)
        {
            this.gridView = gridView;
            this.cellRegistry = cellRegistry;
        }

        void INotifyUnitMoved.UnitMoved(Unit unit, Vector2Int newPosition)
        {
            unit.SetOrder(cellRegistry.Height - newPosition.y);
        }

        void INotifyUnitSelected.UnitSelected(Unit unit)
        {
            var type = unit.Team == Team.Player ? SelectionType.AllyUnit : SelectionType.EnemyUnit;
            gridView.PaintInputCellSelection(unit.Position.ToCenter(), type);
        }

        void INotifyUnitSelected.UnitDeselected()
        {
            gridView.ClearSelection();
        }
    }
}