using UnityEngine;

namespace Game.Combat.Flow.Phases
{
    public class ResultPhase : IPassivePhase
    {
        public bool IsComplete { get; private set; }

        public void Enter()
        {
            Debug.Log("[ResultPhase] Battle completed!");
            IsComplete = false;
        }

        public void Exit()
        {
            IsComplete = true;
        }
    }
}
