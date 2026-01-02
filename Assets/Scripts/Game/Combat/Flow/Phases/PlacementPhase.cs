using Game.Combat.Entities.Grid;
using Game.Combat.Entities.Selector;
using Game.Combat.Entities.Units;
using Game.Combat.Grid;
using Game.Combat.Units;
using Game.Utils;
using UnityEngine;
using VContainer;

namespace Game.Combat.Phases
{
    public class PlacementPhase : ICombatPhase
    {
        private readonly GridView gridView;
        private readonly CellRegistry cellRegistry;
        private readonly UnitRegistry unitRegistry;
        private readonly BoundsInt placementArea;

        private Unit selectedUnit;

        public bool IsComplete { get; private set; }

        public PlacementPhase(GridView gridView, CellRegistry cellRegistry, UnitRegistry unitRegistry, BoundsInt placementArea)
        {
            this.gridView = gridView;
            this.cellRegistry = cellRegistry;
            this.unitRegistry = unitRegistry;
            this.placementArea = placementArea;
        }

        public void Enter()
        {
            gridView.PaintArea(placementArea);
        }

        public void UpdateHover(Vector2Int position, HighlightType type)
        {
            if (!placementArea.Contains((Vector3Int)position)) type = HighlightType.Blocked;
            gridView.PaintHighlight(position.ToCenter(), type);
        }

        public void UpdateClick(Vector2Int position, SelectionType type)
        {
            if (!placementArea.Contains((Vector3Int)position)) return;

            if (type == SelectionType.Unit)
            {
                if (unitRegistry.TryGetUnit(position, out var unit))
                {
                    selectedUnit = unit;
                }
            }
            else if (selectedUnit && type == SelectionType.Free)
            {
                if (unitRegistry.TryMoveUnit(selectedUnit, position))
                {
                    cellRegistry.TrySetBlocked(selectedUnit.Position.ToInt(), false);
                    cellRegistry.TrySetBlocked(position, true);
                    type = SelectionType.Unit;
                }
            }

            gridView.PaintSelection(position.ToCenter(), type);
        }

        public void Exit()
        {
            gridView.ClearArea();
        }

        public void ConfirmPlacement()
        {
            IsComplete = true;
        }
    }
}