using System;
using UnityEngine;

namespace Game.Core.Combat.Setup
{
    [Serializable]
    public class GridData
    {
        public int GridWidth = 34;
        public int GridHeight = 20;

        public BoundsInt PlacementArea;
    }
}
