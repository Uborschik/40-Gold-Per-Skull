using Game.Combat.Entities.Units;
using Game.Unity.Input;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Combat.Selection
{
    public class Selector : IDisposable
    {
        private readonly InputService inputService;
        private readonly HighlightSelector highlightSelector;
        private readonly CellSelector cellSelector;
        private readonly UnitSelector unitSelector;

        private GridCell highlightedCell;
        private Unit selectedUnit;
        private bool isSelected;

        public GridCell SelectedCell => isSelected ? highlightedCell : null;
        public Unit SelectedUnit => selectedUnit;

        public Selector(InputService inputService, HighlightSelector highlightSelector, CellSelector cellSelector, UnitSelector unitSelector)
        {
            Debug.Log($"[Selector] Created");

            this.inputService = inputService;
            this.highlightSelector = highlightSelector;
            this.cellSelector = cellSelector;
            this.unitSelector = unitSelector;

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
            highlightedCell = highlightSelector.OnMousePosition(inputService.GetGridPosition(ctx.ReadValue<Vector2>()));
        }

        private void OnLeftClick(InputAction.CallbackContext ctx)
        {
            if (highlightedCell == null) return;

            isSelected = cellSelector.OnLeftClick(SelectedCell);
            selectedUnit = unitSelector.OnLeftClick(SelectedCell);
        }

        private void OnRightClick(InputAction.CallbackContext ctx)
        {
            isSelected = cellSelector.OnRightClick();
        }
    }
}
