using Game.Combat.Grid;
using Game.Combat.Input;
using Game.Combat.Units;
using Game.Utils;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Combat.Entities.Selector
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
        Unit,
        Free,
        Blocked
    }

    public class Selector : IDisposable
    {
        public event Action<Vector2Int, HighlightType> Hover;
        public event Action<Vector2Int, SelectionType> Click;
        public event Action Reset;

        private readonly InputService inputService;
        private readonly CellRegistry cellRegistry;
        private readonly UnitRegistry unitRegistry;

        private Vector2Int? currentPosition;
        private HighlightType highlightType;
        private SelectionType selectionType;

        public Selector(InputService inputService, CellRegistry cellRegistry, UnitRegistry unitRegistry)
        {
            Debug.Log($"[Selector] Created");

            this.inputService = inputService;
            this.cellRegistry = cellRegistry;
            this.unitRegistry = unitRegistry;

            highlightType = HighlightType.None;
            selectionType = SelectionType.None;

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

            if (cell == null) return;
            if (currentPosition == position) return;

            currentPosition = position;
            highlightType = cell.IsBlocked ? HighlightType.Blocked : HighlightType.Free;

            Hover?.Invoke(currentPosition.Value, highlightType);
        }

        private void OnLeftClick(InputAction.CallbackContext ctx)
        {
            if (currentPosition == null) return;

            var position = currentPosition.Value;

            if (unitRegistry.TryGetUnit(position, out var unit))
                selectionType = SelectionType.Unit;
            else
                selectionType = SelectionType.Free;

            Click?.Invoke(currentPosition.Value, selectionType);
        }

        private void OnRightClick(InputAction.CallbackContext ctx)
        {
            if (currentPosition == null) return;

            currentPosition = null;
            selectionType = SelectionType.None;

            Reset?.Invoke();
        }
    }
}
