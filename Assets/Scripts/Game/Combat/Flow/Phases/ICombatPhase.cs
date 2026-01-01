namespace Game.Combat.Phases
{
    public interface ICombatPhase
    {
        bool IsComplete { get; }
        void Enter();
        void Update();
        void Exit();
    }
}