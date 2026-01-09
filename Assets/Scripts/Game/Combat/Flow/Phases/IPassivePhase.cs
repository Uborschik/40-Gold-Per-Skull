namespace Game.Combat.Flow.Phases
{
    public interface IPassivePhase
    {
        bool IsComplete { get; }
        void Enter();
        void Exit();
    }
}