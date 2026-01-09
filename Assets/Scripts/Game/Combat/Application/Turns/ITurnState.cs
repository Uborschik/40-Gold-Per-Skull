using Cysharp.Threading.Tasks;
using Game.Combat.Entities.Units;

namespace Game.Combat.Application.Turns
{
    public enum TurnType : byte { Player, AI }

    public interface ITurnState
    {
        TurnType Type { get; }

        UniTask Enter(Unit unit);
        UniTask Process();
        void Exit();
    }
}
