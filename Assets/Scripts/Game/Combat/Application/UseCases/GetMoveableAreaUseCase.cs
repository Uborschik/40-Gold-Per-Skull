using Game.Combat.Entities.Grid;
using Game.Combat.Entities.Units;
using Game.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Combat.Application.UseCases
{
    public class GetMoveableAreaUseCase
    {
        private readonly CellRegistry cellRegistry;
        private readonly UnitRegistry unitRegistry;

        public GetMoveableAreaUseCase(CellRegistry cellRegistry, UnitRegistry unitRegistry)
        {
            this.cellRegistry = cellRegistry;
            this.unitRegistry = unitRegistry;
        }

        public HashSet<Vector2Int> Execute(Unit unit)
        {
            var area = new HashSet<Vector2Int>();
            var start = unit.Position.ToInt();
            int maxDistance = unit.Speed;

            var queue = new Queue<(Vector2Int pos, int remaining)>();
            var visited = new HashSet<Vector2Int> { start };

            queue.Enqueue((start, maxDistance));

            while (queue.Count > 0)
            {
                var (current, remaining) = queue.Dequeue();
                area.Add(current);

                if (remaining <= 0) continue;

                foreach (var neighbor in GetNeighbors(current))
                {
                    if (visited.Contains(neighbor)) continue;

                    var cell = cellRegistry.GetCell(neighbor);
                    if (cell == null || !cell.IsWalkable) continue;

                    if (unitRegistry.TryGetUnit(neighbor, out _)) continue;

                    visited.Add(neighbor);
                    queue.Enqueue((neighbor, remaining - 1));
                }
            }

            return area;
        }

        private IEnumerable<Vector2Int> GetNeighbors(Vector2Int pos)
        {
            yield return pos + Vector2Int.up;
            yield return pos + Vector2Int.down;
            yield return pos + Vector2Int.left;
            yield return pos + Vector2Int.right;
        }
    }
}