using Game.Combat.Application.UseCases;
using Game.Combat.Infrastructure.Input;
using Game.Combat.Infrastructure.View;
using Game.Utils;
using UnityEngine;

namespace Game.Combat.Flow.Phases
{
    public class CombatPhase : ICombatPhase
    {
        private readonly GridView gridView;
        private readonly SelectUnitUseCase selectUseCase;
        private readonly EndTurnUseCase endTurnUseCase;

        public bool IsComplete { get; private set; }

        public CombatPhase(
            GridView gridView,
            SelectUnitUseCase selectUseCase,
            EndTurnUseCase endTurnUseCase)
        {
            this.gridView = gridView;
            this.selectUseCase = selectUseCase;
            this.endTurnUseCase = endTurnUseCase;
        }

        public void Enter()
        {
            Debug.Log($"[CombatPhase] Begin");
            IsComplete = false;
        }

        public void UpdateHover(Vector2Int position, HighlightType type)
        {
            gridView.PaintHighlight(position.ToCenter(), type);
        }

        public void UpdateClick(Vector2Int position, SelectionType type)
        {
            selectUseCase.Execute(position, out _);
            gridView.PaintInputCellSelection(position.ToCenter(), type);
        }

        public void Exit()
        {
            IsComplete = true;
        }

        public void Reset()
        {
            gridView.ClearSelection();
        }

        public void EndTurn()
        {
            endTurnUseCase.Execute();
        }
    }
}