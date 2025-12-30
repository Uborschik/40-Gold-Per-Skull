using System.Collections.Generic;

namespace Game.Core.Combat.Units
{
    internal class TeamRegistry
    {
        private readonly Dictionary<UnitTeam, List<UnitModel>> unitsByTeam = new();

        public void Add(UnitModel unit)
        {
            if (unit == null) return;

            if (!unitsByTeam.TryGetValue(unit.Team, out var teamUnits))
            {
                teamUnits = new List<UnitModel>();
                unitsByTeam[unit.Team] = teamUnits;
            }

            teamUnits.Add(unit);
        }

        public bool TryRemove(UnitModel unit)
        {
            if (unit == null) return false;
            if (!unitsByTeam.TryGetValue(unit.Team, out var teamUnits)) return false;

            var removed = teamUnits.Remove(unit);
            if (teamUnits.Count == 0)
                unitsByTeam.Remove(unit.Team);

            return removed;
        }

        public IReadOnlyList<UnitModel> GetTeam(UnitTeam team) =>
            unitsByTeam.TryGetValue(team, out var units) ? units : new List<UnitModel>(0);
    }
}