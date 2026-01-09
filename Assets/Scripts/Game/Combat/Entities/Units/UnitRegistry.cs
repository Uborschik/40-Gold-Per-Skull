using Game.Combat.Core.Entities;
using Game.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Combat.Entities.Units
{
    public class UnitRegistry
    {
        private readonly Dictionary<Vector2Int, Unit> unitMap = new();
        private readonly Dictionary<Team, List<Unit>> teamMap = new();
        private readonly Dictionary<Team, List<Unit>> cachedTeamLists = new();

        private bool cacheInvalidated = true;

        public int Width { get; }
        public int Height { get; }

        public UnitRegistry(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public bool TryGetUnit(Vector2Int position, out Unit unit) { return unitMap.TryGetValue(position, out unit); }

        public List<Unit> GetAllAliveUnits() => unitMap.Values.Where(unit => unit.IsAlive).ToList();

        public IReadOnlyList<Unit> GetTeam(Team team)
        {
            if (cacheInvalidated)
            {
                RebuildCache();
                cacheInvalidated = false;
            }

            if (!cachedTeamLists.ContainsKey(team))
            {
                cachedTeamLists[team] = new List<Unit>(teamMap[team]);
            }

            return cachedTeamLists[team];
        }

        public bool TryAddUnit(Vector2Int position, Unit unit)
        {
            if (!IsValid(position)) return false;
            if (unitMap.ContainsKey(position))
                return false;

            unitMap[position] = unit;

            if (!teamMap.ContainsKey(unit.Team))
                teamMap.Add(unit.Team, new List<Unit>());
            teamMap[unit.Team].Add(unit);

            cacheInvalidated = true;

            return true;
        }

        public bool TryRemoveUnit(Vector2Int position)
        {
            if (!IsValid(position)) return false;
            if (!unitMap.TryGetValue(position, out var unit))
                return false;

            unitMap.Remove(position);

            if (teamMap.TryGetValue(unit.Team, out List<Unit> teamUnits))
            {
                teamUnits.Remove(unit);

                if (teamUnits.Count == 0)
                    teamMap.Remove(unit.Team);
            }

            cacheInvalidated = true;

            return true;
        }

        public bool TryMoveUnit(Unit unit, Vector2Int to)
        {
            if (!IsValid(to)) return false;
            if (unit.Position == to) return false;
            if (unitMap.ContainsKey(to)) return false;

            unitMap.Remove(unit.Position.ToInt());
            unitMap[to] = unit;

            return true;
        }

        private void RebuildCache()
        {
            cachedTeamLists.Clear();

            foreach (var teamPair in teamMap)
                cachedTeamLists[teamPair.Key] = new List<Unit>(teamPair.Value);
        }

        private bool IsValid(Vector2Int position)
        {
            return position.x >= 0 && position.y >= 0 && position.x < Width && position.y < Height;
        }
    }
}
