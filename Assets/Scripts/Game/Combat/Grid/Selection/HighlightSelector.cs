using Game.Combat.Grid;
using Game.Combat.Views;
using UnityEngine;

namespace Game.Combat.Selection
{
    public class HighlightSelector
    {
        private readonly CellRegistry cellRegistry;
        private readonly HighlightView highlightView;

        public HighlightSelector(CellRegistry cellRegistry, HighlightView highlightView)
        {
            this.cellRegistry = cellRegistry;
            this.highlightView = highlightView;
        }

        public GridCell OnMousePosition(Vector2 value)
        {
            //var position = value;

            //if (!cellRegistry.IsValid(position)) return null;

            //var cell = cellRegistry.GetCell(position);
            //highlightView.Set(value.ToCenter2(), cell.IsBlocked);

            return null;
        }
    }
}
