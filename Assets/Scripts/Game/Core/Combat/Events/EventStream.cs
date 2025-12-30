using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Core.Combat.Events
{
    public interface ICombatEvent
    {
    }

    public class EventStream
    {
        private readonly Dictionary<Type, List<object>> subscribers = new();

        public void Publish<T>(T combatEvent) where T : ICombatEvent
        {
            var eventType = typeof(T);

            if (!subscribers.TryGetValue(eventType, out var handlerList)) return;

            var handlers = handlerList.ToArray();

            foreach (var handlerObj in handlers)
            {
                if (handlerObj is Action<T> handler)
                {
                    handler.Invoke(combatEvent);
                }
            }
        }

        public void Subscribe<T>(Action<T> handler) where T : ICombatEvent
        {
            var eventType = typeof(T);
            if (!subscribers.ContainsKey(eventType))
            {
                subscribers[eventType] = new List<object>();
            }

            subscribers[eventType].Add(handler);
        }

        public void Unsubscribe<T>(Action<T> handler) where T : ICombatEvent
        {
            var eventType = typeof(T);
            if (!subscribers.TryGetValue(eventType, out var handlerList)) return;

            handlerList.Remove(handler);
        }
    }
}