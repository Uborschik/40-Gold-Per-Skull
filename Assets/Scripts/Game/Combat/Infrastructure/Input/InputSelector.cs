using Game.Combat.Core.Entities;
using Game.Combat.Entities.Grid;
using Game.Combat.Entities.Units;
using Game.Utils;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Combat.Infrastructure.Input
{
    public enum HighlightType
    {
        None,
        Free,
        Blocked
    }
    public enum SelectionType
    {
        None,
        AllyUnit,
        EnemyUnit,
        Free,
        Blocked
    }

    public class InputSelector : IDisposable
    {
        public event Action<Vector2Int, HighlightType> Hover;
        public event Action<Vector2Int, SelectionType> Click;
        public event Action Reset;

        private readonly InputService inputService;
        private readonly CellRegistry cellRegistry;
        private readonly UnitRegistry unitRegistry;

        private Vector2Int? currentPosition;

        public InputSelector(InputService inputService, CellRegistry cellRegistry, UnitRegistry unitRegistry)
        {
            this.inputService = inputService;
            this.cellRegistry = cellRegistry;
            this.unitRegistry = unitRegistry;

            inputService.MousePosition.performed += OnMousePosition;
            inputService.LeftClick.canceled += OnLeftClick;
            inputService.RightClick.canceled += OnRightClick;
        }

        public void Dispose()
        {
            inputService.MousePosition.performed -= OnMousePosition;
            inputService.LeftClick.canceled -= OnLeftClick;
            inputService.RightClick.canceled -= OnRightClick;
        }

        private void OnMousePosition(InputAction.CallbackContext ctx)
        {
            var position = inputService.GetGridPosition(ctx.ReadValue<Vector2>()).ToInt();
            var cell = cellRegistry.GetCell(position);

            if (cell == null || currentPosition == position) return;

            currentPosition = position;
            var type = cell.IsBlocked ? HighlightType.Blocked : HighlightType.Free;
            Hover?.Invoke(position, type);
        }

        private void OnLeftClick(InputAction.CallbackContext ctx)
        {
            if (currentPosition == null) return;

            var position = currentPosition.Value;
            var selectionType = unitRegistry.TryGetUnit(position, out var unit)
                ? (unit.Team == Team.Player ? SelectionType.AllyUnit : SelectionType.EnemyUnit)
                : SelectionType.Free;

            Click?.Invoke(position, selectionType);
        }

        private void OnRightClick(InputAction.CallbackContext ctx)
        {
            if (currentPosition == null) return;

            currentPosition = null;
            Reset?.Invoke();
        }
    }
}