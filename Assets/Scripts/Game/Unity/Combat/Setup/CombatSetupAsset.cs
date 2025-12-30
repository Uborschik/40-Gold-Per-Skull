using Game.Core.Combat.Setup;
using UnityEngine;

namespace Game.Unity.Combat.Setup
{
    [CreateAssetMenu(fileName = "CombatSetup", menuName = "Combat/Scene Setup")]
    public class CombatSetupAsset : ScriptableObject
    {
        public GridData GridData;
        public UnitDeployerData DeployerData;
    }
}