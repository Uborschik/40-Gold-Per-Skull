using Game.Combat.Infrastructure.Input;
using UnityEngine;

namespace Game.Combat.Flow.Phases
{
    public interface ICombatPhase
    {
        bool IsComplete { get; }
        void Enter();
        public void UpdateHover(Vector2Int position, HighlightType type);
        void UpdateClick(Vector2Int position, SelectionType type);
        void Exit();
        void Reset();
    }
}