using Game.Core.Combat.Events;
using Game.Core.Combat.Grid;
using Game.Core.Combat.Phases;
using Game.Core.Combat.Setup;
using Game.Core.Combat.TurnOrder;
using Game.Core.Combat.Units;
using Game.Unity.Combat.Flow;
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
        [SerializeField] private HoverObjectView hoverObjectView;

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

            builder.RegisterInstance(setupAsset.GridData);
            builder.RegisterInstance(setupAsset.DeployerData);
            builder.RegisterInstance(mainCamera);

            builder.Register<EventStream>(Lifetime.Scoped);
            builder.Register<InputService>(Lifetime.Scoped);
            builder.Register<CellRegistry>(Lifetime.Scoped)
                .AsSelf().AsImplementedInterfaces();
            builder.Register<UnitModelRegistry>(Lifetime.Scoped);
            builder.Register<UnitModelFactory>(Lifetime.Scoped);
            builder.Register<IRandomProvider, SystemRandomProvider>(Lifetime.Scoped);
            builder.Register<TurnQueue>(Lifetime.Scoped);

            builder.RegisterComponentInNewPrefab(gridViewPrefab, Lifetime.Scoped)
               .AsSelf().AsImplementedInterfaces();

            builder.Register(r => r.Resolve<GridView>().HighlightView, Lifetime.Scoped);
            builder.Register(r => r.Resolve<GridView>().SelectionView, Lifetime.Scoped);

            builder.RegisterComponentInNewPrefab(hoverObjectView, Lifetime.Scoped)
               .AsSelf().AsImplementedInterfaces();

            builder.Register<UnitDeployer>(Lifetime.Scoped);
            builder.Register<PlacementPhase>(Lifetime.Scoped);
            builder.Register<CombatPhase>(Lifetime.Scoped);
            builder.Register<CombatFlow>(Lifetime.Scoped);

            builder.RegisterEntryPoint<CombatSceneLifetime>();
        }
    }
}