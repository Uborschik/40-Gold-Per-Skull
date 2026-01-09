using UnityEngine;

namespace Game.Combat.Core.Entities
{
    public class Health
    {
        public int Current { get; private set; }
        public int Max { get; }

        public bool IsDead => Current <= 0;

        public Health(int max)
        {
            Max = max;
            Current = max;
        }

        public void TakeDamage(int amount)
        {
            Current = Mathf.Max(0, Current - amount);
        }

        public void Heal(int amount)
        {
            Current = Mathf.Min(Max, Current + amount);
        }
    }
}