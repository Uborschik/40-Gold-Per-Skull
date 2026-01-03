using Game.Combat.Entities.Grid;
using Game.Combat.Setup;

namespace Game.Combat.Infrastructure.Factories
{
    public class CellFactory
    {
        private readonly CellRegistry cellRegistry;
        private readonly GridData data;

        public CellFactory(CellRegistry cellRegistry, GridData data)
        {
            this.cellRegistry = cellRegistry;
            this.data = data;
        }

        public void Create()
        {
            var grid = new GridCell[data.GridWidth, data.GridHeight];
            for (int y = 0; y < grid.GetLength(1); y++)
                for (int x = 0; x < grid.GetLength(0); x++)
                    grid[x, y] = new GridCell(x, y);
            
            cellRegistry.AddGrid(grid);
        }
    }
}
