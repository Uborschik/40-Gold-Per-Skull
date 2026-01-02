using Cysharp.Threading.Tasks;
using Game.Combat.Entities.Factories;
using Game.Combat.Entities.Grid;
using Game.Combat.Phases;
using System.Threading;
using UnityEngine;
using VContainer.Unity;

namespace Game.Combat.Flow
{
    public class CombatSceneLifetime : IAsyncStartable
    {
        private readonly GridView gridView;
        private readonly CellFactory cellFactory;
        private readonly UnitFactory unitFactory;
        private readonly CombatFlow combatFlow;

        public CombatSceneLifetime(
            GridView gridView,
            CellFactory cellFactory,
            UnitFactory unitFactory,
            CombatFlow combatFlow)
        {
            this.gridView = gridView;
            this.cellFactory = cellFactory;
            this.unitFactory = unitFactory;
            this.combatFlow = combatFlow;
        }

        public async Awaitable StartAsync(CancellationToken cancellation)
        {
            Debug.Log("[Combat] Starting async initialization...");

            cellFactory.Create();
            await UniTask.Yield(PlayerLoopTiming.PostLateUpdate);

            gridView.PaintGrid();
            await UniTask.Yield(PlayerLoopTiming.PostLateUpdate);

            unitFactory.CreateTeams();
            await UniTask.Yield(PlayerLoopTiming.PostLateUpdate);

            combatFlow.Start();

            Debug.Log("[Combat] Initialization complete!");
        }
    }
}