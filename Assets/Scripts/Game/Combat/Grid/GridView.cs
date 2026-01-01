using Game.Combat.Grid;
using Game.Unity.Combat.Views;
using UnityEngine;
using UnityEngine.Tilemaps;
using VContainer;

namespace Game.Combat.Views
{
    public class GridView : MonoBehaviour
    {
        [Inject] private readonly CellRegistry cellRegistry;

        [SerializeField] private Tilemap mainTilemap;
        [SerializeField] private Tilemap selectionTilemap;

        [SerializeField] private TileBase groundTile;
        [SerializeField] private TileBase selectionTile;

        [SerializeField] private HighlightView highlightView;
        [SerializeField] private CellSelectionView selectionView;

        public HighlightView HighlightView => highlightView;
        public CellSelectionView SelectionView => selectionView;

        public void PaintGrid()
        {
            for (int y = 0; y < cellRegistry.Height; y++)
                for (int x = 0; x < cellRegistry.Width; x++)
                {
                    var position = new Vector3Int(x, y);
                    mainTilemap.SetTile(position, groundTile);
                }
        }

        public void ClearGrid()
        {
            mainTilemap.ClearAllTiles();
        }

        public void PaintArea(BoundsInt area)
        {
        }

        public void ClearArea()
        {
            selectionTilemap.ClearAllTiles();
        }
    }
}
