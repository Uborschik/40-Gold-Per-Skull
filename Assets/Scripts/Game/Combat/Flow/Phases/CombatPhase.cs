using Game.Combat.Application.Events;
using Game.Combat.Application.Turns;
using Game.Combat.Core.Entities;
using Game.Combat.Infrastructure.Input;
using Game.Combat.Infrastructure.View;
using Game.Utils;
using UnityEngine;

namespace Game.Combat.Flow.Phases
{
    public class CombatPhase : IInteractivePhase
    {
        private readonly GridView gridView;
        private readonly BattleCyclic turnController;
        private readonly IEventBus events;

        public bool IsComplete { get; private set; }

        public CombatPhase(
            GridView gridView,
            BattleCyclic turnController,
            IEventBus events)
        {
            this.gridView = gridView;
            this.turnController = turnController;
            this.events = events;

            events.Subscribe<ShowMoveableAreaRequested>(OnShowMovableArea);
            events.Subscribe<HideMoveableAreaRequested>(OnHideMovableArea);
        }

        public void Enter()
        {
            Debug.Log("[NewCombatPhase] Начало боевой фазы");
            IsComplete = false;

            turnController.InitializeBattle();
        }

        public void Exit()
        {
            IsComplete = true;
            OnHideMovableArea(new HideMoveableAreaRequested());
            OnHideAttackRange(new HideAttackRangeRequested());

            Debug.Log("[NewCombatPhase] Конец боевой фазы");
        }

        public void UpdateHover(Vector2Int position, HighlightType type) { }

        public void UpdateClick(Vector2Int position, SelectionType type) { }

        public void Reset() { }

        private void OnShowMovableArea(ShowMoveableAreaRequested evt)
        {
            gridView.PaintMovableArea(evt.Positions);
        }

        private void OnHideMovableArea(HideMoveableAreaRequested evt)
        {
            gridView.ClearMovableArea();
        }

        private void OnShowAttackRange(ShowAttackRangeRequested evt)
        {
            gridView.PaintMovableArea(evt.Positions);
        }

        private void OnHideAttackRange(HideAttackRangeRequested evt)
        {
            gridView.ClearMovableArea();
        }
    }
}