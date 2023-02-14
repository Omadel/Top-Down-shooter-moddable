using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilesInfo : MonoBehaviour
{
    public Tile RepaintTile;
    public Vector3Int SelectedTilePosition;
    public Vector3Int OldTilePosition;
    public TileBase OldTile;
    public TileBase currentTile;
  
    public void TilePreview()
    {
        SelectedTilePosition = MapEditor.Instance.MousePositionToCellPosition();
        currentTile = MapEditor.Instance.GetTilemap().GetTile(SelectedTilePosition);
        RepaintTile = MapEditor.Instance._RepaintTile;
        if (SelectedTilePosition != OldTilePosition)
        {
            if(OldTile != null)
            {
                MapEditor.Instance.GetTilemap().SetTile(OldTilePosition, OldTile);
            }
            OldTilePosition = SelectedTilePosition;
            OldTile = currentTile;
        }
        MapEditor.Instance.GetTilemap().SetTile(SelectedTilePosition, RepaintTile);
    }
    public void ResetOldTileData()
    {
        OldTile = null;
        OldTilePosition = SelectedTilePosition;
    }
}
