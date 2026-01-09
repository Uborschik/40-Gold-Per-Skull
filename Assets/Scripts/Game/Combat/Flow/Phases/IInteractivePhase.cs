using Game.Combat.Infrastructure.Input;
using UnityEngine;

namespace Game.Combat.Flow.Phases
{
    public interface IInteractivePhase : IPassivePhase
    {
        void UpdateHover(Vector2Int position, HighlightType type);
        void UpdateClick(Vector2Int position, SelectionType type);
        void Reset();
    }
}