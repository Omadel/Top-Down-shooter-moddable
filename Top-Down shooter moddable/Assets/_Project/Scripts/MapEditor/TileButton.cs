using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class TileButton : MonoBehaviour
{
    public Tile _TileButton;
    public void SelectTileButton()
    {
        MapEditor.Instance._RepaintTile = _TileButton;
    }
    
}
