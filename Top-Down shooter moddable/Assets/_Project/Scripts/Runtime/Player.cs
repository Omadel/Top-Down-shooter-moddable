using System;
using System.IO;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerStats stats;

    [System.Serializable]
    private class PlayerStats
    {
        public string Name = "Etienne";
        public int Health = 100;
        public float Speed = 5f;
        public float FireRate = .5f;
        public int Damage = 10;
    }

    private void Awake()
    {
        string path = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}/My Games/{Application.productName}";
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        LoadPlayerSprite(path);
        LoadPlayerStats(path);
    }

    private void LoadPlayerStats(string path)
    {
        string playerStatsPath = path + "/PlayerStats.json";
        if (!File.Exists(playerStatsPath))
        {
            string json = JsonUtility.ToJson(stats, true);
            File.WriteAllText(playerStatsPath, json);
        }
        else
        {
            stats = JsonUtility.FromJson<PlayerStats>(File.ReadAllText(playerStatsPath));
        }
    }

    private void LoadPlayerSprite(string path)
    {
        string playerSpritePath = path + "/PlayerSprite.png";
        if (File.Exists(playerSpritePath)) GetComponent<SpriteRenderer>().sprite = LoadSprite(playerSpritePath);
    }

    private Sprite LoadSprite(string path)
    {
        if (!File.Exists(path))
        {
            Debug.LogError("No SpriteFound !");
            return null;
        }
        Texture2D spriteTexture = new Texture2D(1, 1);
        spriteTexture.LoadImage(File.ReadAllBytes(path));
        spriteTexture.filterMode = FilterMode.Point;
        int pixelsPerUnit = Mathf.Max(spriteTexture.width, spriteTexture.height);
        var sprite =Sprite.Create(spriteTexture, new Rect(0, 0, spriteTexture.width, spriteTexture.height), new Vector2(.5f, .5f), pixelsPerUnit);
        sprite.name = Path.GetFileNameWithoutExtension(path);
        return sprite;
    }
}
