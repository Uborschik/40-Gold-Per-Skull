using Game.Core.Combat.Grid;
using Game.Core.Combat.Units;

namespace Game.Core.Combat.Phases
{
    public interface ICombatPhase
    {
        UnitModel SelectedUnit { get; }
        bool IsComplete { get; }

        void Enter();
        void Update();
        void Exit();
        bool TrySelect(Position2Int position);
        bool TryPlaceSelected(Position2Int newPosition);
        void ConfirmPlacement();
    }
}