namespace Game.Combat.Application.Events
{
    public interface IEventBus
    {
        void Subscribe<TEvent>(IEventListener<TEvent> listener);
        void Unsubscribe<TEvent>(IEventListener<TEvent> listener);
        void Publish<TEvent>(TEvent evt);
    }
}