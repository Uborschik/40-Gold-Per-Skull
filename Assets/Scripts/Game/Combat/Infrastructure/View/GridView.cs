using Game.Combat.Entities.Grid;
using Game.Combat.Infrastructure.Input;
using Game.Utils;
using UnityEngine;
using UnityEngine.Tilemaps;
using VContainer;

namespace Game.Combat.Infrastructure.View
{
    public class GridView : MonoBehaviour
    {
        [Inject] private readonly CellRegistry cellRegistry;

        [SerializeField] private Tilemap mainTilemap;
        [SerializeField] private Tilemap selectionTilemap;

        [SerializeField] private TileBase groundTile;
        [SerializeField] private TileBase selectionTile;

        [SerializeField] private HighlightSelectionView highlightSelection;
        [SerializeField] private InputCellSelectionView inputCellSelection;
        [SerializeField] private TurnCellSelectionView turnCellSelection;

        public void PaintGrid()
        {
            foreach (var position in cellRegistry.Grid.AllCells())
            {
                mainTilemap.SetTile(position, groundTile);
            }
        }

        public void PaintArea(BoundsInt area)
        {
            foreach (var position in area.AllCells())
            {
                selectionTilemap.SetTile(position, selectionTile);
            }
        }

        public void PaintHighlight(Vector2 position, HighlightType type) => highlightSelection.Set(position, type);
        public void PaintInputCellSelection(Vector2 position, SelectionType type) => inputCellSelection.Set(position, type);
        public void PaintTurnCellSelection(Vector2 position, SelectionType type) => turnCellSelection.Set(position, type);

        public void ClearGrid() => mainTilemap.ClearAllTiles();
        public void ClearArea() => selectionTilemap.ClearAllTiles();
        public void ClearHighlight() => highlightSelection.Hide();
        public void ClearSelection() => inputCellSelection.Hide();
    }
}
