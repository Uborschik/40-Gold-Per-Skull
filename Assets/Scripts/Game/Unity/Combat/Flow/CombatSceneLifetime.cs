using Cysharp.Threading.Tasks;
using Game.Core.Combat.Phases;
using Game.Unity.Combat.Views;
using System.Threading;
using UnityEngine;
using VContainer.Unity;

namespace Game.Unity.Combat.Flow
{
    public class CombatSceneLifetime : IAsyncStartable
    {
        private readonly GridView gridView;
        private readonly HoverObjectView hoverObjectView;
        private readonly UnitDeployer deployer;
        private readonly CombatFlow combatFlow;

        public CombatSceneLifetime(
            GridView gridView,
            HoverObjectView hoverObjectView,
            UnitDeployer deployer,
            CombatFlow combatFlow)
        {
            this.gridView = gridView;
            this.hoverObjectView = hoverObjectView;
            this.deployer = deployer;
            this.combatFlow = combatFlow;
        }

        public async Awaitable StartAsync(CancellationToken cancellation)
        {
            Debug.Log("[Combat] Starting async initialization...");

            gridView.PaintGrid();
            hoverObjectView.Initialize();
            await UniTask.Yield(PlayerLoopTiming.PostLateUpdate);

            deployer.DeployInitial();
            await UniTask.Yield(PlayerLoopTiming.PostLateUpdate);

            combatFlow.Start();

            Debug.Log("[Combat] Initialization complete!");
        }
    }
}