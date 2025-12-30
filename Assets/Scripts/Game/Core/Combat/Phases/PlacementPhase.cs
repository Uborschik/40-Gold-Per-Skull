using Game.Core.Combat.Events;
using Game.Core.Combat.Grid;
using Game.Core.Combat.Setup;
using Game.Core.Combat.Units;
using Game.Core.Combat.Views;

namespace Game.Core.Combat.Phases
{
    public class PlacementPhase : ICombatPhase
    {
        private readonly EventStream eventStream;
        private readonly UnitModelRegistry modelRegistry;
        private readonly IAreaView areaView;
        private readonly IArea placementArea;

        private UnitModel selectedUnit;

        public bool IsComplete { get; private set; }
        public UnitModel SelectedUnit => selectedUnit;

        public PlacementPhase(EventStream eventStream, UnitModelRegistry modelRegistry, IAreaView areaView, GridData gridData)
        {
            this.eventStream = eventStream;
            this.modelRegistry = modelRegistry;
            this.areaView = areaView;
            placementArea = gridData.PlacementArea;
        }

        public void Enter() { areaView.PaintArea(placementArea); }
        public void Update() { /* Ожидание внешних действий */ }
        public void Exit() { areaView.ClearArea(); }

        public bool TrySelect(Position2Int position)
        {
            if (!placementArea.Contains(position)) return false;

            var unit = modelRegistry.GetUnitAt(position);

            if (unit == null) return false;
            if (unit.Team != UnitTeam.Player) return false;

            selectedUnit = unit;

            eventStream.Publish(new UnitSelectedEvent(unit.ID, unit.Position));
            return true;
        }

        public bool TryPlaceSelected(Position2Int newPosition)
        {
            if (!placementArea.Contains(newPosition)) return false;
            if (modelRegistry.GetUnitAt(newPosition) != null) return false;

            var oldPosition = SelectedUnit.Position;

            modelRegistry.TryMoveUnit(selectedUnit.ID, newPosition);
            eventStream.Publish(new UnitMovedEvent(selectedUnit.ID, oldPosition, newPosition));

            return true;
        }

        public void ConfirmPlacement()
        {
            IsComplete = true;
            eventStream.Publish(new PhaseChangedEvent(this));
        }
    }
}