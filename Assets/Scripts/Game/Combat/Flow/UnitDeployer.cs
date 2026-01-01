using Game.Combat.Entities.Units;
using Game.Combat.Grid;
using Game.Core.Combat.Setup;
using Game.Unity.Combat.Setup;
using System.Linq;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.Combat.Flow
{
    public class UnitDeployer
    {
        private readonly IObjectResolver objectResolver;
        private readonly UnitFactory unitFactory;
        private readonly CellRegistry cellRegistry;
        private readonly UnitDeployerData data;

        public UnitDeployer(IObjectResolver objectResolver, UnitFactory unitFactory, CellRegistry cellRegistry, UnitDeployerData data)
        {
            this.objectResolver = objectResolver;
            this.unitFactory = unitFactory;
            this.cellRegistry = cellRegistry;
            this.data = data;
        }

        public void DeployInitial()
        {
            var oddRows = Enumerable.Range(0, cellRegistry.Height).Where(y => y % 2 == 1).ToArray();

            for (int i = 0; i < data.TeamData.Length; i++)
            {
                var teamData = data.TeamData[i];
                var parent = new GameObject(teamData.Team + "Team").transform;
                var x = teamData.Team == Team.Player ? 0 : cellRegistry.Width - 1;

                for (int j = 0; j < Mathf.Min(teamData.Units.Length, oddRows.Length); j++)
                {
                    DeployUnit(new Vector2Int(x, oddRows[j]), teamData.Team, teamData.Units[j], parent);
                }
            }
        }

        private void DeployUnit(Vector2Int gridPosition, Team team, UnitData unitData, Transform parent)
        {
            var result = unitFactory.Create(gridPosition, team, unitData);

            var view = objectResolver.Instantiate(data.Prefab, parent);
            view.Initialize();
        }
    }
}