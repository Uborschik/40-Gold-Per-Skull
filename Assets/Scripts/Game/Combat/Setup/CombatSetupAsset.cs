using UnityEngine;

namespace Game.Combat.Setup
{
    [CreateAssetMenu(fileName = "CombatSetup", menuName = "Combat/Scene Setup")]
    public class CombatSetupAsset : ScriptableObject
    {
        public GridData GridData;
        public UnitFactoryData UnitFactoryData;
    }
}