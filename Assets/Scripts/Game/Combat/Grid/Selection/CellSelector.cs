using Game.Unity.Combat.Views;
using Game.Utils;
using UnityEngine;

namespace Game.Combat.Selection
{
    public class CellSelector
    {
        private readonly CellSelectionView cellSelectionView;

        public CellSelector(CellSelectionView cellSelectionView)
        {
            this.cellSelectionView = cellSelectionView;
        }

        public bool OnLeftClick(GridCell cell)
        {
            var position = new Vector2(cell.X, cell.Y);
            cellSelectionView.Set(position.ToCenter(), cell.IsBlocked);
            return true;
        }

        public bool OnRightClick()
        {
            cellSelectionView.Hide();
            return false;
        }
    }
}
