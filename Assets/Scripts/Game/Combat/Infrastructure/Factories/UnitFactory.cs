using Game.Combat.Core.Entities;
using Game.Combat.Entities.Grid;
using Game.Combat.Entities.Units;
using Game.Combat.Setup;
using Game.Utils;
using System.Linq;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.Combat.Infrastructure.Factories
{
    public class UnitFactory
    {
        private readonly IObjectResolver objectResolver;
        private readonly UnitRegistry unitRegistry;
        private readonly CellRegistry cellRegistry;
        private readonly UnitFactoryData data;

        public UnitFactory(IObjectResolver objectResolver, UnitRegistry unitRegistry, CellRegistry cellRegistry, UnitFactoryData data)
        {
            this.objectResolver = objectResolver;
            this.unitRegistry = unitRegistry;
            this.cellRegistry = cellRegistry;
            this.data = data;
        }

        public void CreateTeams()
        {
            var oddRows = Enumerable.Range(0, cellRegistry.Height).Where(y => y % 2 == 1).ToArray();

            for (int i = 0; i < data.TeamData.Length; i++)
            {
                var teamData = data.TeamData[i];
                var parent = new GameObject(teamData.Team + "Team").transform;
                var x = teamData.Team == Team.Player ? 0 : cellRegistry.Width - 1;

                for (int j = 0; j < Mathf.Min(teamData.Units.Length, oddRows.Length); j++)
                {
                    CreateUnit(new Vector2Int(x, oddRows[j]), teamData.Team, teamData.Units[j], parent);
                }
            }
        }

        private void CreateUnit(Vector2Int position, Team team, UnitData unitData, Transform parent)
        {
            var stats = new Stats(unitData.Strength, unitData.Dexterity, unitData.Constitution);
            var unit = objectResolver.Instantiate(data.Prefab, parent);
            var health = new Health(10);
            var ap = new ActionPoints(1);
            unit.Initialize(unitData.Name, health, ap, 3, 6, stats, team, position.ToCenter());
            unitRegistry.TryAddUnit(position, unit);
            cellRegistry.TrySetBlocked(position, true);
        }
    }
}