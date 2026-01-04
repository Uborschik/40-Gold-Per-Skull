using Game.Combat.Application.Events;
using Game.Combat.Application.UseCases;
using Game.Combat.Entities.Grid;
using Game.Combat.Entities.Units;
using Game.Combat.Flow;
using Game.Combat.Flow.Commands;
using Game.Combat.Flow.Mediators;
using Game.Combat.Flow.Phases;
using Game.Combat.Infrastructure.Events;
using Game.Combat.Infrastructure.Factories;
using Game.Combat.Infrastructure.Input;
using Game.Combat.Infrastructure.Systems;
using Game.Combat.Infrastructure.TurnOrder;
using Game.Combat.Infrastructure.View;
using Game.Combat.Setup;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

namespace Game.Combat
{
    public class CombatInstaller : LifetimeScope
    {
        [Space(10f)]
        [SerializeField] private CombatSetupAsset setupAsset;
        [SerializeField] private Camera mainCamera;

        [Header("UI")]
        [SerializeField] private Button phaseBtn;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<CellRegistry>(Lifetime.Singleton);
            builder.Register<UnitRegistry>(Lifetime.Singleton)
                .WithParameter(setupAsset.GridData.GridWidth)
                .WithParameter(setupAsset.GridData.GridHeight);
            builder.Register<InputService>(Lifetime.Singleton)
                .WithParameter(mainCamera);
            builder.Register<IEventBus, EventBus>(Lifetime.Singleton);

            builder.Register<EndPhaseCommand>(Lifetime.Scoped);
            builder.Register<EndTurnCommand>(Lifetime.Scoped);

            builder.Register<ICommandFactory, CommandFactory>(Lifetime.Scoped);

            builder.Register<PhaseButtonMediator>(Lifetime.Scoped)
                .AsSelf()
                .AsImplementedInterfaces()
                .WithParameter(phaseBtn);

            builder.RegisterComponentInHierarchy<GridView>();
            builder.RegisterComponentInHierarchy<UIHandler>();

            builder.Register<CellFactory>(Lifetime.Scoped)
                .WithParameter(setupAsset.GridData);
            builder.Register<UnitFactory>(Lifetime.Scoped)
                .WithParameter(setupAsset.UnitFactoryData);

            builder.Register<IRandomProvider, SystemRandomProvider>(Lifetime.Scoped);
            builder.Register<TurnQueue>(Lifetime.Scoped);

            builder.Register<TurnSystem>(Lifetime.Scoped);
            builder.Register<GridViewUpdater>(Lifetime.Scoped);

            builder.Register<SelectUnitUseCase>(Lifetime.Scoped);
            builder.Register<MoveUnitUseCase>(Lifetime.Scoped);
            builder.Register<EndTurnUseCase>(Lifetime.Scoped);

            builder.Register<PlacementPhase>(Lifetime.Scoped)
                .WithParameter(setupAsset.GridData.PlacementArea);
            builder.Register<CombatPhase>(Lifetime.Scoped);

            builder.Register<InputSelector>(Lifetime.Scoped);

            builder.Register<CombatFlow>(Lifetime.Scoped)
                .WithParameter(resolver => new ICombatPhase[]
                { resolver.Resolve<PlacementPhase>(), resolver.Resolve<CombatPhase>() });

            builder.RegisterEntryPoint<CombatSceneLifetime>(Lifetime.Scoped);
        }
    }
}