using Game.Combat.Application.Notifications;
using Game.Combat.Entities.Units;
using UnityEngine;

namespace Game.Combat.Application.UseCases
{
    public class SelectUnitUseCase
    {
        private readonly UnitRegistry unitRegistry;
        private readonly INotifyUnitSelected[] listeners;

        public SelectUnitUseCase(
            UnitRegistry unitRegistry,
            INotifyUnitSelected[] listeners)
        {
            this.unitRegistry = unitRegistry;
            this.listeners = listeners;
        }

        public bool Execute(Vector2Int position, out Unit selectedUnit)
        {
            if (unitRegistry.TryGetUnit(position, out selectedUnit))
            {
                foreach (var listener in listeners)
                    listener.UnitSelected(selectedUnit);
                return true;
            }

            selectedUnit = null;
            foreach (var listener in listeners)
                listener.UnitDeselected();
            return false;
        }
    }
}