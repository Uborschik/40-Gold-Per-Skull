using System;
using UnityEngine;

namespace Utils
{
    public static class Directions2D
    {
        public static readonly Vector2Int[] cardinalDirections = new Vector2Int[]
        {
            new(0, 1),
            new(1, 0),
            new(0, -1),
            new(-1, 0)
        };

        public static readonly Vector2Int[] diagonalDirections = new Vector2Int[]
        {
            new( 1,  1),
            new(-1,  1),
            new(-1, -1),
            new( 1, -1)
        };

        public static readonly Vector2Int[] eightDirections = new Vector2Int[]
        {
            new( 0,  1),
            new( 1,  1),
            new( 1,  0),
            new( 1, -1),
            new( 0, -1),
            new(-1, -1),
            new(-1,  0),
            new(-1,  1)
        };
    }
}