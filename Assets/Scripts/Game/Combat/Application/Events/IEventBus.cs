using System;

namespace Game.Combat.Application.Events
{
    public interface IEventBus
    {
        void Subscribe<TEvent>(Action<TEvent> listener);
        void Unsubscribe<TEvent>(Action<TEvent> listener);
        void Publish<TEvent>(TEvent evt);
    }
}