using Game.Combat.Application.Events;
using Game.Combat.Core.Entities;
using Game.Combat.Entities.Grid;
using Game.Combat.Infrastructure.Input;
using Game.Combat.Infrastructure.View;
using Game.Utils;
using System;

namespace Game.Combat.Infrastructure.Systems
{
    public class GridViewUpdater : IDisposable
    {
        private readonly IEventBus eventBus;
        private readonly GridView gridView;
        private readonly CellRegistry cellRegistry;

        public GridViewUpdater(IEventBus eventBus, GridView gridView, CellRegistry cellRegistry)
        {
            this.eventBus = eventBus;
            this.gridView = gridView;
            this.cellRegistry = cellRegistry;

            eventBus.Subscribe<UnitSelected>(SetSelectionType);
        }

        public void Dispose()
        {
            eventBus.Unsubscribe<UnitSelected>(SetSelectionType);
        }

        public void SetSelectionType(UnitSelected evt)
        {
            var type = evt.Unit.Team == Team.Player ? SelectionType.Ally : SelectionType.Enemy;
            gridView.PaintInputCellSelection(evt.Unit.Position.ToCenter(), type);
        }
    }
}