using Game.Combat.Views;
using Game.Core.Combat.Setup;
using Game.Core.Combat.TurnOrder;
using Game.Unity.Combat.Setup;
using Game.Unity.Combat.Views;
using Game.Unity.Input;
using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.Unity.Combat.Installers
{
    public class CombatInstaller : LifetimeScope
    {
        [Space(10f)]
        [SerializeField] private CombatSetupAsset setupAsset;
        [SerializeField] private Camera mainCamera;

        [Header("Prefabs")]
        [SerializeField] private GridView gridViewPrefab;
        [SerializeField] private HighlightView highlightView;
        [SerializeField] private CellSelectionView cellSelectionView;
        [SerializeField] private UnitSelectionView unitSelectionView;

        protected override void Configure(IContainerBuilder builder)
        {
            var validator = new CombatSetupValidator();
            var errors = validator.Validate(setupAsset.GridData, setupAsset.DeployerData.TeamData);

            if (errors.Count > 0)
            {
                var errorMessage = $"CombatSetup validation failed:\n{string.Join("\n", errors)}";
                Debug.LogError(errorMessage);
                throw new InvalidOperationException(errorMessage);
            }
        }
    }
}