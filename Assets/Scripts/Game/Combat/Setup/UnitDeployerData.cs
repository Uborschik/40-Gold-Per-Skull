using Game.Combat.Entities.Units;
using Game.Core.Combat.Setup;
using System;

namespace Game.Unity.Combat.Setup
{
    [Serializable]
    public class UnitDeployerData
    {
        public TeamData[] TeamData;
        public Unit Prefab;
    }
}
