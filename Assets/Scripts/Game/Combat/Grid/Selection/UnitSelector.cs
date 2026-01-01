using Game.Combat.Entities.Units;
using Game.Combat.Units;
using Game.Unity.Combat.Views;
using Game.Utils;
using UnityEngine;

namespace Game.Combat.Selection
{
    public class UnitSelector
    {
        private readonly UnitRegistry unitRegistry;
        private readonly UnitSelectionView unitSelectionView;

        public UnitSelector(UnitRegistry unitRegistry, UnitSelectionView unitSelectionView)
        {
            this.unitRegistry = unitRegistry;
            this.unitSelectionView = unitSelectionView;
        }

        public Unit OnLeftClick(GridCell cell)
        {
            var position = new Vector2Int(cell.X, cell.Y);

            if (unitRegistry.TryGetUnit(position, out var unit))
            {
                unitSelectionView.Set(position.ToCenter());
                return unit;
            }

            return null;
        }
    }
}
