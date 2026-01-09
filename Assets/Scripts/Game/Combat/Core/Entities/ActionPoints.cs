using System;

namespace Game.Combat.Core.Entities
{
    public class ActionPoints
    {
        public int Max { get; private set; } = 1;
        public int Current { get; private set; }

        public bool HasActions => Current > 0;

        public ActionPoints(int max)
        {
            Max = max;
        }

        public void Refresh() => Current = Max;
        public bool Spend()
        {
            if (HasActions)
            {
                Current--;
                return true;
            }
            return false;
        }
    }
}