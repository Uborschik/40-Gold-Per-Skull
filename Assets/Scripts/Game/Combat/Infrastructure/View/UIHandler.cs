using Game.Combat.Flow.Mediators;
using UnityEngine;
using VContainer;

namespace Game.Combat.Infrastructure.View
{
    public class UIHandler : MonoBehaviour
    {
        [Inject] private readonly PhaseButtonMediator phaseButtonMediator;

        private void OnDisable()
        {
            phaseButtonMediator.Dispose();
        }
    }
}