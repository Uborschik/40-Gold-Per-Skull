using Game.Combat.Application.Events;
using System.Collections.Generic;

namespace Game.Combat.Infrastructure.Events
{
    public class EventBus : IEventBus
    {
        private readonly Dictionary<System.Type, List<object>> subscribers = new();

        public void Subscribe<TEvent>(IEventListener<TEvent> listener)
        {
            var key = typeof(TEvent);
            if (!subscribers.ContainsKey(key))
                subscribers[key] = new List<object>();

            subscribers[key].Add(listener);
        }

        public void Unsubscribe<TEvent>(IEventListener<TEvent> listener)
        {
            var key = typeof(TEvent);
            if (subscribers.TryGetValue(key, out var list))
                list.Remove(listener);
        }

        public void Publish<TEvent>(TEvent evt)
        {
            var key = typeof(TEvent);
            if (!subscribers.TryGetValue(key, out var list)) return;

            foreach (var listener in list.ToArray())
            {
                (listener as IEventListener<TEvent>)?.OnEvent(evt);
            }
        }
    }
}