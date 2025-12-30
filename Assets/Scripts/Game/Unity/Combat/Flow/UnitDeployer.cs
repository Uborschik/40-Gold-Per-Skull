using Game.Core.Combat.Grid;
using Game.Core.Combat.Setup;
using Game.Core.Combat.Units;
using Game.Unity.Combat.Setup;
using System.Linq;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.Unity.Combat.Flow
{
    public class UnitDeployer
    {
        private readonly IObjectResolver objectResolver;
        private readonly UnitModelFactory modelFactory;
        private readonly CellRegistry cellRegistry;
        private readonly UnitDeployerData data;

        public UnitDeployer(IObjectResolver objectResolver, UnitModelFactory modelFactory, CellRegistry cellRegistry, UnitDeployerData data)
        {
            this.objectResolver = objectResolver;
            this.modelFactory = modelFactory;
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
                var x = teamData.Team == UnitTeam.Player ? 0 : cellRegistry.Width - 1;

                for (int j = 0; j < Mathf.Min(teamData.Units.Length, oddRows.Length); j++)
                {
                    DeployUnit(new Position2Int(x, oddRows[j]), teamData.Team, teamData.Units[j], parent);
                }
            }
        }

        private void DeployUnit(Position2Int gridPosition, UnitTeam team, UnitData unitData, Transform parent)
        {
            var result = modelFactory.Spawn(gridPosition, team, unitData);

            if (!result.IsSuccess)
            {
                Debug.LogError($"Failed to spawn unit at {gridPosition}: {result.Error}");
                return;
            }

            var view = objectResolver.Instantiate(data.Prefab, parent);
            view.Initialize(result.Value.ID, result.Value.Position, result.Value.Team);
        }
    }
}