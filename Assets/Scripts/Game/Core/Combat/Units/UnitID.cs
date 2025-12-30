using System;

namespace Game.Core.Combat.Units
{
    public readonly struct UnitID
    {
        public uint Value { get; }
        public UnitID(uint value) => Value = value;
        public override string ToString() => $"Unit:{Value}";
    }
}