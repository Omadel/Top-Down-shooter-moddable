using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.IO;
using System;

public class MapInfos : MonoBehaviour
{
    private int MapSize;
    MapData mapData = new MapData();
    public List<TileBase> Tiles;
    private TileBase TileBasePrefab;
    public MapLoader mapLoader;

    

    // Start is called before the first frame update
    void Start()
    {
        MapSize = MapEditor.Instance._MapSize;
    }

    public void SaveData(string fileN)
    {
        MapData mapData2 = new MapData();
        

        Vector3Int[] positions = new Vector3Int[MapSize * MapSize];
        for (int i = 0; i < (MapSize * MapSize); i++)
        {
            positions[i] = new Vector3Int(i % MapSize, i / MapSize, 0);
            Tiles.Add(TileBasePrefab);
            Tiles[i] = MapEditor.Instance.GetTilemap().GetTile(positions[i]);
            if(Tiles[i] != null)
                mapData2.tiles.Add(new TileData { x = positions[i].x, y = positions[i].y, index = KeepDigit(Tiles[i].name) -36 });
        }
        string jsonString = JsonUtility.ToJson(mapData2);
        string folderPath = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/My Games/" + Application.productName + "/Maps";
        string fileName = fileN;
        string filePath = folderPath + "/" + fileName;


        //Vector3Int[] positions = new Vector3Int[MapSize * MapSize];
        

        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);
        
        File.WriteAllText(filePath, jsonString);
    }
    public int KeepDigit(string tileName)
    {
        string index = "";
        foreach(char c in tileName)
        {
            if(char.IsDigit(c))
            {
                index += c;
            }
        }
        return int.Parse(index);
    }
    public void LoadNewMap()
    {
        int version = 0;
        string folderPath = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/My Games/" + Application.productName + "/Maps";
        string fileName = "map";
        string filePath = folderPath + "/" + fileName;

        while (File.Exists(filePath))
        {
            version++;
            filePath = folderPath + "/" + Path.GetFileNameWithoutExtension(fileName) + version + ".json";
        }
        string jsonString = "";
        // Write the JSON string to a file
        File.WriteAllText(filePath, jsonString);
        foreach (Vector3Int position in MapEditor.Instance.GetTilemap().cellBounds.allPositionsWithin)
        {
            TileBase tile = MapEditor.Instance._Tiles[14];
            MapEditor.Instance.GetTilemap().SetTile(position, tile);
        }
    }
    public void LoadMap(string selectedFile)
    {
       
        string jsonString = File.ReadAllText(selectedFile);

        MapData mapData2 = JsonUtility.FromJson<MapData>(jsonString);
        foreach (TileData tileData in mapData2.tiles)
        {
            TileBase tile = MapEditor.Instance._Tiles[tileData.index];
            
            Vector3Int position = new Vector3Int(tileData.x, tileData.y, 0);
            MapEditor.Instance.GetTilemap().SetTile(position, tile);
        }

    }
    public void InstantiateSavedMap()
    {
        Vector3Int[] positions = new Vector3Int[MapSize * MapSize];
        for (int i = 0; i < (MapSize * MapSize); i++)
        {
            positions[i] = new Vector3Int(i % MapSize, i / MapSize, 0);
            MapEditor.Instance.GetTilemap().SetTile(positions[i], Tiles[i]);
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
