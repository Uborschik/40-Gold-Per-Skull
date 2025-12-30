using Game.Core.Combat.Setup;
using Game.Unity.Combat.Views;
using System;

namespace Game.Unity.Combat.Setup
{
    [Serializable]
    public class UnitDeployerData
    {
        public TeamData[] TeamData;
        public UnitView Prefab;
    }
}
