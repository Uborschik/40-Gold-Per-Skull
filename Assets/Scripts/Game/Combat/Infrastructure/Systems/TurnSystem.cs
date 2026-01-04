using Game.Combat.Application.Events;
using Game.Combat.Core.Entities;
using Game.Combat.Entities.Units;
using Game.Combat.Infrastructure.Input;
using Game.Combat.Infrastructure.TurnOrder;
using Game.Combat.Infrastructure.View;
using Game.Utils;
using System;

namespace Game.Combat.Infrastructure.Systems
{
    public class TurnSystem : IEventListener<TurnChanged>, IEventListener<UnitMoved>, IDisposable
    {
        private readonly IEventBus events;
        private readonly TurnQueue turnQueue;
        private readonly UnitRegistry unitRegistry;
        private readonly GridView gridView;

        public TurnSystem(
            IEventBus events,
            TurnQueue turnQueue,
            UnitRegistry unitRegistry,
            GridView gridView)
        {
            this.events = events;
            this.turnQueue = turnQueue;
            this.unitRegistry = unitRegistry;
            this.gridView = gridView;

            events.Subscribe<TurnChanged>(this);
            events.Subscribe<UnitMoved>(this);
        }

        public void Dispose()
        {
            events.Unsubscribe<TurnChanged>(this);
            events.Unsubscribe<UnitMoved>(this);
        }

        public void OnEvent(TurnChanged evt)
        {
            HighlightCurrentUnit(evt.CurrentUnit);
            CheckVictory();
        }

        public void OnEvent(UnitMoved evt)
        {
            evt.Unit.SetOrder(unitRegistry.Height - evt.NewPosition.y);
        }

        public void StartNewRound()
        {
            var aliveUnits = unitRegistry.GetAllAliveUnits();
            turnQueue.Build(aliveUnits);
            HighlightCurrentUnit(turnQueue.Current);
        }

        private void HighlightCurrentUnit(Unit unit)
        {
            if (unit == null) return;

            var type = unit.Team == Team.Player ? SelectionType.AllyUnit : SelectionType.EnemyUnit;
            gridView.PaintTurnCellSelection(unit.Position.ToCenter(), type);
        }

        private Team? CheckVictory()
        {
            var playerUnits = unitRegistry.GetTeam(Team.Player);
            var enemyUnits = unitRegistry.GetTeam(Team.Enemy);

            if (playerUnits.Count == 0) return Team.Enemy;
            if (enemyUnits.Count == 0) return Team.Player;

            return null;
        }
    }
}