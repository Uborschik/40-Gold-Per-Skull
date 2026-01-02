using Game.Combat.Entities.Selector;
using Game.Combat.Grid;
using Game.Utils;
using UnityEngine;
using UnityEngine.Tilemaps;
using VContainer;

namespace Game.Combat.Entities.Grid
{
    public class GridView : MonoBehaviour
    {
        [Inject] private readonly CellRegistry cellRegistry;

        [SerializeField] private Tilemap mainTilemap;
        [SerializeField] private Tilemap selectionTilemap;

        [SerializeField] private TileBase groundTile;
        [SerializeField] private TileBase selectionTile;

        [SerializeField] private HighlightView highlightView;
        [SerializeField] private SelectionView selectionView;

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

        public void PaintHighlight(Vector2 position, HighlightType type) => highlightView.Set(position, type);

        public void PaintSelection(Vector2 position, SelectionType type) => selectionView.Set(position, type);

        public void ClearGrid() => mainTilemap.ClearAllTiles();
        public void ClearArea() => selectionTilemap.ClearAllTiles();
        public void ClearHighlight() => highlightView.Hide();
        public void ClearSelection() => selectionView.Hide();
    }
}
