using Cysharp.Threading.Tasks;
using Game.Combat.Phases;
using Game.Combat.Views;
using Game.Unity.Combat.Views;
using System.Threading;
using UnityEngine;
using VContainer.Unity;

namespace Game.Combat.Flow
{
    public class CombatSceneLifetime : IAsyncStartable
    {
        private readonly GridView gridView;
        private readonly UnitDeployer deployer;
        private readonly CombatFlow combatFlow;

        public CombatSceneLifetime(
            GridView gridView,
            UnitDeployer deployer,
            CombatFlow combatFlow)
        {
            this.gridView = gridView;
            this.deployer = deployer;
            this.combatFlow = combatFlow;
        }

        public async Awaitable StartAsync(CancellationToken cancellation)
        {
            Debug.Log("[Combat] Starting async initialization...");

            gridView.PaintGrid();
            await UniTask.Yield(PlayerLoopTiming.PostLateUpdate);

            deployer.DeployInitial();
            await UniTask.Yield(PlayerLoopTiming.PostLateUpdate);

            combatFlow.Start();

            Debug.Log("[Combat] Initialization complete!");
        }
    }
}