namespace Game.Combat.Application.Events
{
    public interface IEventListener<in TEvent>
    {
        void OnEvent(TEvent evt);
    }
}