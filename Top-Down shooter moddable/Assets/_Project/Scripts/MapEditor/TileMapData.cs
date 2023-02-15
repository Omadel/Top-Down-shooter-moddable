using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.IO;
using UnityEngine.Tilemaps;
public class TileMapData : MonoBehaviour
{
    public TMP_Dropdown fileDropdown;
    string path;
    string mapFolderPath = "/Maps";
    string fileType = "*.json";
    public List<Tile> _Tiles;
    public Tilemap _Tilemap;
    public string currentFile;
    string defaultFile;
    // Start is called before the first frame update
    void Awake()
    {
        path = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/My Games/" + Application.productName; ;
        string mapPath = path + mapFolderPath;
       
        string[] files = Directory.GetFiles(mapPath, fileType);
        defaultFile = files[0];
        // Create an array of just the file names without the path
        string[] fileNames = new string[files.Length];
        for (int i = 0; i < files.Length; i++)
        {
            fileNames[i] = Path.GetFileName(files[i]);
        }

        // Create a list of Dropdown.OptionData objects from the file names
        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();
        foreach (string fileName in fileNames)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData(fileName);
            options.Add(option);
        }

        // Set the options for the dropdown
        fileDropdown.ClearOptions();
        fileDropdown.AddOptions(options);
        LoadDefaultMap();
        
    }
    private void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        currentFile = path + mapFolderPath + "/" + fileDropdown.captionText.text;
    }
    public void LoadDefaultMap()
    {
        string jsonString = File.ReadAllText(defaultFile);
        MapData mapData2 = JsonUtility.FromJson<MapData>(jsonString);
        foreach (TileData tileData in mapData2.tiles)
        {
            TileBase tile = _Tiles[tileData.index];

            Vector3Int position = new Vector3Int(tileData.x, tileData.y, 0);
            _Tilemap.SetTile(position, tile);
        }
    }
    public void LoadMap()
    {
        currentFile = path + mapFolderPath + "/" + fileDropdown.captionText.text;
        string jsonString = File.ReadAllText(currentFile);
        MapData mapData2 = JsonUtility.FromJson<MapData>(jsonString);
        foreach (TileData tileData in mapData2.tiles)
        {
            TileBase tile = _Tiles[tileData.index];

            Vector3Int position = new Vector3Int(tileData.x, tileData.y, 0);
            _Tilemap.SetTile(position, tile);
        }
    }

    [System.Serializable]
    private class TileData
    {
        public int x;
        public int y;

        public int index;
    }
    [System.Serializable]
    private class MapData
    {
        public List<TileData> tiles = new List<TileData>();
    }
}
