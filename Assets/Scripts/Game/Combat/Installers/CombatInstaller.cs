using Game.Combat.Application.Notifications;
using Game.Combat.Application.UseCases;
using Game.Combat.Entities.Grid;
using Game.Combat.Entities.Units;
using Game.Combat.Flow;
using Game.Combat.Flow.Commands;
using Game.Combat.Flow.Mediators;
using Game.Combat.Flow.Phases;
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

            builder.Register<PhaseButtonMediator>(Lifetime.Scoped)
                .WithParameter(phaseBtn);

            builder.Register<ICommandFactory, CommandFactory>(Lifetime.Scoped);

            builder.Register<EndPhaseCommand>(Lifetime.Scoped);
            builder.Register<EndTurnCommand>(Lifetime.Scoped);

            builder.RegisterComponentInHierarchy<GridView>();

            builder.Register<CellFactory>(Lifetime.Scoped)
                .WithParameter(setupAsset.GridData);
            builder.Register<UnitFactory>(Lifetime.Scoped)
                .WithParameter(setupAsset.UnitFactoryData);

            builder.Register<IRandomProvider, SystemRandomProvider>(Lifetime.Scoped);
            builder.Register<TurnQueue>(Lifetime.Scoped);

            builder.Register<TurnSystem>(Lifetime.Scoped);
            builder.Register<GridViewUpdater>(Lifetime.Scoped);

            builder.Register(resolver =>
            {
                var listeners = new INotifyUnitSelected[]
                {
                    resolver.Resolve<GridViewUpdater>()
                };

                return new SelectUnitUseCase(resolver.Resolve<UnitRegistry>(), listeners);
            }, Lifetime.Scoped);

            builder.Register(resolver =>
            {
                var listeners = new INotifyUnitMoved[]
                {
                    resolver.Resolve<TurnSystem>(),
                    resolver.Resolve<GridViewUpdater>()
                };

                return new MoveUnitUseCase(
                    resolver.Resolve<CellRegistry>(),
                    resolver.Resolve<UnitRegistry>(),
                    listeners
                );
            }, Lifetime.Scoped);

            builder.Register(resolver =>
            {
                var listeners = new INotifyUnitSelected[]
                {
                    resolver.Resolve<GridViewUpdater>()
                };

                var turnChangedListeners = new INotifyTurnChanged[]
                {
                    resolver.Resolve<TurnSystem>() // ✅ Добавить!
                };

                return new EndTurnUseCase(resolver.Resolve<TurnQueue>(), listeners, turnChangedListeners);
            }, Lifetime.Scoped);

            builder.Register(resolver =>
            {
                return new PlacementPhase(
                    resolver.Resolve<GridView>(),
                    setupAsset.GridData.PlacementArea,
                    resolver.Resolve<SelectUnitUseCase>(),
                    resolver.Resolve<MoveUnitUseCase>()
                );
            }, Lifetime.Scoped);

            builder.Register<CombatPhase>(Lifetime.Scoped);

            builder.Register<InputSelector>(Lifetime.Scoped);

            builder.Register(resolver =>
            {
                var phases = new ICombatPhase[]
                {
                    resolver.Resolve<PlacementPhase>(),
                    resolver.Resolve<CombatPhase>()
                };

                var phaseListeners = new INotifyPhaseChanged[]
                {
                    resolver.Resolve<TurnSystem>(),
                    resolver.Resolve<PhaseButtonMediator>()
                };

                return new CombatFlow(
                    resolver.Resolve<InputSelector>(),
                    phases,
                    phaseListeners
                );
            }, Lifetime.Scoped);

            builder.RegisterEntryPoint<CombatSceneLifetime>(Lifetime.Scoped);
        }
    }
}