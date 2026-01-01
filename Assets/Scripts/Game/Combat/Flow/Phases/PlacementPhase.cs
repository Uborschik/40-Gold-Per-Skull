using Game.Combat.Units;
using Game.Core.Combat.Setup;
using UnityEngine;

namespace Game.Combat.Phases
{
    public class PlacementPhase : ICombatPhase
    {
        private readonly UnitRegistry unitRegistry;

        public bool IsComplete { get; private set; }

        public PlacementPhase(UnitRegistry unitRegistry, GridData gridData)
        {
            this.unitRegistry = unitRegistry;
        }

        public void Enter() {}
        public void Update() {}
        public void Exit() {}

        public bool TryPlaceSelected(Vector2Int newPosition)
        {
            return true;
        }

        public void ConfirmPlacement()
        {
            IsComplete = true;
        }
    }
}