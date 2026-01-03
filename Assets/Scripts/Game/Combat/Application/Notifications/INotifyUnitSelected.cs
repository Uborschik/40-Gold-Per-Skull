using Game.Combat.Entities.Units;

namespace Game.Combat.Application.Notifications
{
    public interface INotifyUnitSelected
    {
        void UnitSelected(Unit unit);
        void UnitDeselected();
    }
}