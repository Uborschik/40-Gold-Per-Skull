using Game.Core.Combat.Events;
using Game.Core.Combat.Grid;
using Game.Core.Combat.Views;
using Game.Unity.Utils;
using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using VContainer;

namespace Game.Unity.Combat.Views
{
    public class GridView : MonoBehaviour, IAreaView
    {
        [Inject] private readonly CellRegistry cellRegistry;

        [SerializeField] private Tilemap mainTilemap;
        [SerializeField] private Tilemap selectionTilemap;

        [SerializeField] private TileBase groundTile;
        [SerializeField] private TileBase selectionTile;

        [SerializeField] private HighlightView highlightView;
        [SerializeField] private SelectionView selectionView;

        public HighlightView HighlightView => highlightView;
        public SelectionView SelectionView => selectionView;

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

        public void PaintArea(IArea area)
        {
            foreach (var pos in area.AllCells())
            {
                if (!cellRegistry.IsValid(pos)) continue;

                selectionTilemap.SetTile(pos.ToVector3Int(), selectionTile);
            }
        }

        public void ClearArea()
        {
            selectionTilemap.ClearAllTiles();
        }
    }
}
