using Game.Combat.Core.Entities;
using Game.Combat.Entities.Units;

namespace Game.Combat.Application.Notifications
{
    public interface INotifyUnitDied
    {
        void UnitDied(Unit unit);
        void Victory(Team winner);
    }
}