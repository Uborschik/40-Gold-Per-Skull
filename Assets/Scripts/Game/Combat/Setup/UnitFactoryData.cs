using Game.Combat.Entities.Units;
using System;

namespace Game.Combat.Setup
{
    [Serializable]
    public class UnitFactoryData
    {
        public TeamData[] TeamData;
        public Unit Prefab;
    }
}
