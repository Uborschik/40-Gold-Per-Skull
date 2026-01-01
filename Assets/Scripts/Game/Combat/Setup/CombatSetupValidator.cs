using System.Collections.Generic;

namespace Game.Core.Combat.Setup
{
    public interface ICombatSetupValidator
    {
        IReadOnlyList<string> Validate(GridData gridData, TeamData[] teamData);
    }

    public class CombatSetupValidator : ICombatSetupValidator
    {
        public IReadOnlyList<string> Validate(GridData gridData, TeamData[] teamData)
        {
            var errors = new List<string>();

            if (gridData.GridWidth <= 0 || gridData.GridHeight <= 0)
                errors.Add("Grid dimensions must be positive.");

            if (gridData.PlacementArea.min.x >= gridData.PlacementArea.max.x ||
                gridData.PlacementArea.min.y >= gridData.PlacementArea.max.y)
                errors.Add("PlacementArea has invalid bounds (Min >= Max).");

            if (gridData.PlacementArea.min.x < 0 || gridData.PlacementArea.min.y < 0 ||
                gridData.PlacementArea.max.x > gridData.GridWidth ||
                gridData.PlacementArea.max.y > gridData.GridHeight)
                errors.Add("PlacementArea extends beyond grid boundaries.");
            
            if (teamData == null || teamData.Length == 0)
                errors.Add("No team data defined.");

            foreach (var data in teamData)
            {
                if (data.Units == null || data.Units.Length == 0)
                    errors.Add($"Team {data.Team} has no units defined.");

                foreach (var unit in data.Units)
                {
                    //if (string.IsNullOrWhiteSpace(unit.UnitId))
                    //    errors.Add("Unit has empty UnitId.");

                    if (unit.Strength < 0 || unit.Dexterity < 0 || unit.Constitution < 0)
                        errors.Add($"Unit {unit.UnitId} has negative stat values.");
                }
            }

            return errors;
        }
    }
}