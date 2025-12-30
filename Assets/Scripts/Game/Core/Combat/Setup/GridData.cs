using Game.Core.Combat.Grid;
using System;

namespace Game.Core.Combat.Setup
{
    [Serializable]
    public class GridData
    {
        public int GridWidth = 34;
        public int GridHeight = 20;

        public RectangleArea PlacementArea;
    }
}
