using Game.Combat.Application;
using Game.Combat.Application.Events;
using Game.Combat.Application.Turns; // Новый namespace
using Game.Combat.Application.UseCases;
using Game.Combat.Entities.Grid;
using Game.Combat.Entities.Units;
using Game.Combat.Flow;
using Game.Combat.Flow.Commands;
using Game.Combat.Flow.Phases;
using Game.Combat.Infrastructure.Events;
using Game.Combat.Infrastructure.Factories;
using Game.Combat.Infrastructure.Input;
using Game.Combat.Infrastructure.TurnOrder;
using Game.Combat.Infrastructure.View;
using Game.Combat.Setup;
using Game.Combat.UI.View;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.Combat
{
    public class CombatInstaller : LifetimeScope
    {
        [Space(10f)]
        [SerializeField] private CombatSetupAsset setupAsset;
        [SerializeField] private Camera mainCamera;

        protected override void Configure(IContainerBuilder builder)
        {
            // Core регистры
            builder.Register<CellRegistry>(Lifetime.Singleton);
            builder.Register<UnitRegistry>(Lifetime.Singleton)
                .WithParameter(setupAsset.GridData.GridWidth)
                .WithParameter(setupAsset.GridData.GridHeight);

            // Input
            builder.Register<InputService>(Lifetime.Singleton)
                .WithParameter(mainCamera);
            builder.Register<InputSelector>(Lifetime.Scoped);

            // Events
            builder.Register<IEventBus, EventBus>(Lifetime.Singleton);

            // Random
            builder.Register<IRandomProvider, SystemRandomProvider>(Lifetime.Scoped);

            builder.Register<EndPhaseCommand>(Lifetime.Scoped);
            builder.Register<EndTurnCommand>(Lifetime.Scoped);
            builder.Register<ICommandFactory, CommandFactory>(Lifetime.Scoped);

            // View
            builder.RegisterComponentInHierarchy<GridView>();
            builder.RegisterComponentInHierarchy<PhaseButton>();

            // Factories
            builder.Register<CellFactory>(Lifetime.Scoped)
                .WithParameter(setupAsset.GridData);
            builder.Register<UnitFactory>(Lifetime.Scoped)
                .WithParameter(setupAsset.UnitFactoryData);

            builder.Register<SelectUnitUseCase>(Lifetime.Scoped);
            builder.Register<MoveUnitUseCase>(Lifetime.Scoped);
            builder.Register<EndTurnUseCase>(Lifetime.Scoped);

            builder.Register<PlacementPhase>(Lifetime.Scoped).WithParameter(setupAsset.GridData.PlacementArea);

            // Turn Order
            builder.Register<TurnQueueFactory>(Lifetime.Scoped);

            builder.Register<PlayerTurnState>(Lifetime.Transient);
            builder.Register<AITurnState>(Lifetime.Transient);
            builder.Register<TurnStateMachine>(Lifetime.Scoped);

            // ✓ НОВАЯ СИСТЕМА ХОДОВ
            builder.Register<BattleCyclic>(Lifetime.Scoped);
            builder.Register<CombatPhase>(Lifetime.Scoped);

            builder.Register<CombatFlow>(Lifetime.Scoped);

            // ✓ УДАЛЕНО: Все старые классы
            // builder.Register<TurnDirector>(Lifetime.Scoped);
            // builder.Register<GridViewUpdater>(Lifetime.Scoped);
            // builder.Register<AttackUnitUseCase>(Lifetime.Scoped);
            // builder.Register<GetMoveableAreaUseCase>(Lifetime.Scoped);
            // builder.Register<TurnValidator>(Lifetime.Scoped);
            // builder.Register<CombatTurnWorkflow>(Lifetime.Scoped);
            // builder.Register<CombatPhase>(Lifetime.Scoped);
            // builder.Register<ResultPhase>(Lifetime.Scoped);

            // ✓ ИЗМЕНЕНО: Обновленная точка входа
            builder.RegisterEntryPoint<CombatSceneLifetime>(Lifetime.Scoped);
        }
    }
}