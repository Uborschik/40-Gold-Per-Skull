using Game.Combat.Application.Events;
using Game.Combat.Core.Entities;
using Game.Combat.Entities.Grid;
using Game.Combat.Infrastructure.Input;
using Game.Combat.Infrastructure.View;
using Game.Utils;
using System;

namespace Game.Combat.Infrastructure.Systems
{
    public class GridViewUpdater : IEventListener<UnitMoved>, IEventListener<UnitSelected>, IDisposable
    {
        private readonly IEventBus eventBus;
        private readonly GridView gridView;
        private readonly CellRegistry cellRegistry;

        public GridViewUpdater(IEventBus eventBus, GridView gridView, CellRegistry cellRegistry)
        {
            this.eventBus = eventBus;
            this.gridView = gridView;
            this.cellRegistry = cellRegistry;

            eventBus.Subscribe<UnitMoved>(this);
            eventBus.Subscribe<UnitSelected>(this);
        }

        public void Dispose()
        {
            eventBus.Unsubscribe<UnitMoved>(this);
            eventBus.Unsubscribe<UnitSelected>(this);
        }

        public void OnEvent(UnitMoved evt)
        {
            evt.Unit.SetOrder(cellRegistry.Height - evt.NewPosition.y);
        }

        public void OnEvent(UnitSelected evt)
        {
            var type = evt.Unit.Team == Team.Player ? SelectionType.AllyUnit : SelectionType.EnemyUnit;
            gridView.PaintInputCellSelection(evt.Unit.Position.ToCenter(), type);
        }
    }
}