using System;

namespace Game.Core.Combat.Grid
{
    [Serializable]
    public struct Position2Int
    {
        public int X;
        public int Y;
        public Position2Int(int x, int y) => (X, Y) = (x, y);

        public readonly int ChebyshevDistance(Position2Int other) => Math.Max(Math.Abs(X - other.X), Math.Abs(Y - other.Y));
        public readonly int ManhattanDistance(Position2Int other) => Math.Abs(X - other.X) + Math.Abs(Y - other.Y);

        public static bool operator ==(Position2Int a, Position2Int b) => a.X == b.X && a.Y == b.Y;
        public static bool operator !=(Position2Int a, Position2Int b) => !(a == b);
        public override readonly bool Equals(object obj) => obj is Position2Int other && this == other;
        public override readonly int GetHashCode() => HashCode.Combine(X, Y);

        public override string ToString()
        {
            return $"({X}, {Y})";
        }
    }
}