using Game.Combat.Flow.Phases;

namespace Game.Combat.Application.Notifications
{
    public interface INotifyPhaseChanged
    {
        void PhaseEntered(ICombatPhase phase);
        void PhaseExited(ICombatPhase phase);
    }
}