using Game.Combat.Entities.Units;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Combat.Units
{
    public class UnitRegistry
    {
        private readonly Dictionary<Vector2Int, Unit> unitMap = new();
        private readonly Dictionary<Team, List<Unit>> teamMap = new();

        public bool TryGetUnit(Vector2Int position, out Unit unit) { return unitMap.TryGetValue(position, out unit); }

        public List<Unit> GetAllAliveUnits() => unitMap.Values.Where(unit => unit.IsAlive).ToList();

        public List<Unit> GetTeam(Team team) => teamMap.TryGetValue(team, out var units) ? units : new List<Unit>();

        public bool TryAddUnit(Vector2Int position, Unit unit)
        {
            if (unitMap.ContainsKey(position))
                return false;

            unitMap[position] = unit;

            if (!teamMap.ContainsKey(unit.Team))
                teamMap.Add(unit.Team, new List<Unit>());
            teamMap[unit.Team].Add(unit);

            unit.SetPosition(position);

            return true;
        }

        public bool TryRemoveUnit(Vector2Int position)
        {
            if (!unitMap.TryGetValue(position, out var unit))
                return false;

            unitMap.Remove(position);

            if (teamMap.TryGetValue(unit.Team, out List<Unit> teamUnits))
            {
                teamUnits.Remove(unit);

                if (teamUnits.Count == 0)
                    teamMap.Remove(unit.Team);
            }

            return true;
        }

        public bool TryMoveUnit(Vector2Int from, Vector2Int to)
        {
            if (from == to) return false;
            if (!unitMap.TryGetValue(from, out var unit)) return false;
            if (unitMap.TryGetValue(to, out _)) return false;

            unitMap.Remove(from);
            unitMap[to] = unit;
            unit.SetPosition(to);

            return true;
        }
    }
}
