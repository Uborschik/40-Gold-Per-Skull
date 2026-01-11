using Game.Combat.Application.Events;
using Game.Combat.Application.Turns;
using Game.Combat.Infrastructure.Input;
using Game.Combat.Infrastructure.View;
using Game.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Combat.Flow.Phases
{
    public class CombatPhase : IInteractivePhase, IDisposable
    {
        private readonly GridView gridView;
        private readonly BattleCyclic turnController;
        private readonly IEventBus events;

        private HashSet<Vector2Int> area = new();

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

        public void Dispose()
        {
            events.Unsubscribe<ShowMoveableAreaRequested>(OnShowMovableArea);
            events.Unsubscribe<HideMoveableAreaRequested>(OnHideMovableArea);
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

        public void UpdateHover(Vector2Int position, HighlightType type)
        {
            if (area == null || !area.Contains(position)) type = HighlightType.Blocked;

            gridView.PaintHighlight(position.ToCenter(), type);
        }

        public void UpdateClick(Vector2Int position, SelectionType type)
        {
            if (area.Contains(position))
            {
                // Передать в Player-ход
            }
            else
            {
                type = SelectionType.Blocked;
            }

            gridView.PaintInputCellSelection(position.ToCenter(), type);
        }

        public void Reset() => gridView.ClearSelection();

        private void OnShowMovableArea(ShowMoveableAreaRequested evt)
        {
            area.Clear();
            area = evt.Positions;
            gridView.PaintArea(evt.Positions);
        }

        private void OnHideMovableArea(HideMoveableAreaRequested evt)=> gridView.ClearArea();
        private void OnShowAttackRange(ShowAttackRangeRequested evt) => gridView.PaintArea(evt.Positions);
        private void OnHideAttackRange(HideAttackRangeRequested evt) => gridView.ClearArea();
    }
}