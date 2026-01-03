using Game.Combat.Entities.Units;

namespace Game.Combat.Application.Notifications
{
    public interface INotifyTurnChanged
    {
        void TurnChanged(Unit currentUnit);
    }
}
