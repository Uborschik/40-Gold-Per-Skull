using Game.Combat.Application.UseCases;
using Game.Combat.Entities.Units;
using Game.Combat.Infrastructure.Input;
using Game.Combat.Infrastructure.View;
using Game.Utils;
using System.Linq;
using UnityEngine;

namespace Game.Combat.Flow.Phases
{
    public class PlacementPhase : IInteractivePhase
    {
        private readonly GridView gridView;
        private readonly BoundsInt placementArea;
        private readonly SelectUnitUseCase selectUseCase;
        private readonly MoveUnitUseCase moveUseCase;

        private Unit selectedUnit;
        public bool IsComplete { get; private set; }

        public PlacementPhase(
            GridView gridView,
            BoundsInt placementArea,
            SelectUnitUseCase selectUseCase,
            MoveUnitUseCase moveUseCase)
        {
            this.gridView = gridView;
            this.placementArea = placementArea;
            this.selectUseCase = selectUseCase;
            this.moveUseCase = moveUseCase;
        }

        public void Enter()
        {
            IsComplete = false;
            gridView.PaintArea(placementArea);
        }

        public void UpdateHover(Vector2Int position, HighlightType type)
        {
            if (!placementArea.Contains((Vector3Int)position))
                type = HighlightType.Blocked;
            gridView.PaintHighlight(position.ToCenter(), type);
        }

        public void UpdateClick(Vector2Int position, SelectionType type)
        {
            if (!placementArea.Contains((Vector3Int)position)) return;

            if (selectedUnit == null)
            {
                selectUseCase.Execute(position, out selectedUnit);
            }
            else
            {
                if (moveUseCase.Execute(selectedUnit, position))
                {
                    selectedUnit = null;
                }
            }

            gridView.PaintInputCellSelection(position.ToCenter(), type);
        }

        public void Exit()
        {
            IsComplete = true;
            gridView.ClearArea();
            selectedUnit = null;
        }

        public void Reset()
        {
            selectedUnit = null;
            gridView.ClearSelection();
        }
    }
}