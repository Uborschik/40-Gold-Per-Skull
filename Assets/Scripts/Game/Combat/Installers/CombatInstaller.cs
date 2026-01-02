using Game.Combat.Entities.Factories;
using Game.Combat.Entities.Grid;
using Game.Combat.Entities.Selector;
using Game.Combat.Flow;
using Game.Combat.Grid;
using Game.Combat.Input;
using Game.Combat.Phases;
using Game.Combat.Setup;
using Game.Combat.TurnOrder;
using Game.Combat.Units;
using Game.Core.Combat.TurnOrder;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

namespace Game.Unity.Combat.Installers
{
    public class CombatInstaller : LifetimeScope
    {
        [Space(10f)]
        [SerializeField] private CombatSetupAsset setupAsset;
        [SerializeField] private Camera mainCamera;

        [Header("UI")]
        [SerializeField] private Button exitPhaseBtn;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<InputActions>(Lifetime.Singleton);
            builder.Register<InputService>(Lifetime.Scoped)
                .WithParameter(mainCamera);

            builder.Register<CellRegistry>(Lifetime.Scoped);
            builder.Register<UnitRegistry>(Lifetime.Scoped);

            builder.Register<CellFactory>(Lifetime.Scoped)
                .WithParameter(setupAsset.GridData);
            builder.Register<UnitFactory>(Lifetime.Scoped)
                .WithParameter(setupAsset.UnitFactoryData);

            builder.RegisterComponentInHierarchy<GridView>();
            builder.Register<Selector>(Lifetime.Scoped);

            builder.Register<PlacementPhase>(Lifetime.Scoped)
                .WithParameter(setupAsset.GridData.PlacementArea);
            builder.Register<CombatPhase>(Lifetime.Scoped);
            builder.Register<CombatFlow>(Lifetime.Scoped)
                .WithParameter(exitPhaseBtn);

            builder.Register<IRandomProvider, SystemRandomProvider>(Lifetime.Scoped);
            builder.Register<InitiativeRoller>(Lifetime.Scoped);
            builder.Register<TurnQueue>(Lifetime.Scoped);

            builder.RegisterEntryPoint<CombatSceneLifetime>(Lifetime.Scoped);
        }
    }
}