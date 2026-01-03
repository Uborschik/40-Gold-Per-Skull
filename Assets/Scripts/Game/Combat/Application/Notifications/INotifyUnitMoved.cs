using Game.Combat.Entities.Units;
using UnityEngine;

namespace Game.Combat.Application.Notifications
{
    public interface INotifyUnitMoved
    {
        void UnitMoved(Unit unit, Vector2Int newPosition);
    }
}