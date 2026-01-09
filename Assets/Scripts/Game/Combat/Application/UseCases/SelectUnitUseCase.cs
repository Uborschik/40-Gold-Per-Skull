using Game.Combat.Application.Events;
using Game.Combat.Entities.Units;
using UnityEngine;

namespace Game.Combat.Application.UseCases
{
    public class SelectUnitUseCase
    {
        private readonly UnitRegistry unitRegistry;
        private readonly IEventBus events;

        public SelectUnitUseCase(UnitRegistry unitRegistry, IEventBus events)
        {
            this.unitRegistry = unitRegistry;
            this.events = events;
        }

        public bool Execute(Vector2Int position, out Unit selectedUnit)
        {
            if (unitRegistry.TryGetUnit(position, out selectedUnit))
            {
                events.Publish(new UnitSelected(selectedUnit));
                return true;
            }

            selectedUnit = null;
            return false;
        }
    }
}