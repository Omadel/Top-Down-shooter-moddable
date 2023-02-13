using System;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerStats stats;
    [SerializeField] private Sprite bulletSprite;
    [SerializeField] private TMPro.TextMeshProUGUI nameTextMesh;
    private Vector2 direction;
    private float shootTimer;

    [System.Serializable]
    private class PlayerStats
    {
        public string Name = "Etienne";
        public int Health = 100;
        public float Speed = 5f;
        public float FireRate = .5f;
        public int Damage = 10;
        public Bullet.BulletStats[] BulletStats = new Bullet.BulletStats[] { };

    }

    private void Awake()
    {
        string path = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}/My Games/{Application.productName}";
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        LoadPlayerSprite(path + "/PlayerSprite.png");
        LoadBulletSprite(path + "/BulletSprite.png");
        LoadPlayerStats(path + "/PlayerStats.json");
    }

    private void Start()
    {
        nameTextMesh.text = stats.Name;
    }

    private void LoadPlayerSprite(string path)
    {
        if (File.Exists(path)) GetComponent<SpriteRenderer>().sprite = LoadSprite(path);
    }

    private void LoadBulletSprite(string path)
    {
        if (File.Exists(path)) bulletSprite = LoadSprite(path, 32);
    }

    private void LoadPlayerStats(string path)
    {
        if (!File.Exists(path)) File.WriteAllText(path, JsonUtility.ToJson(stats, true));
        else stats = JsonUtility.FromJson<PlayerStats>(File.ReadAllText(path));
    }

    private Sprite LoadSprite(string path, int? pixelsPerUnit = null)
    {
        if (!File.Exists(path))
        {
            Debug.LogError("No SpriteFound !");
            return null;
        }
        Texture2D spriteTexture = new Texture2D(1, 1);
        spriteTexture.LoadImage(File.ReadAllBytes(path));
        spriteTexture.filterMode = FilterMode.Point;
        if (pixelsPerUnit == null) pixelsPerUnit = Mathf.Max(spriteTexture.width, spriteTexture.height);
        Sprite sprite = Sprite.Create(spriteTexture, new Rect(0, 0, spriteTexture.width, spriteTexture.height), new Vector2(.5f, .5f), pixelsPerUnit.Value);
        sprite.name = Path.GetFileNameWithoutExtension(path);
        return sprite;
    }

    private void OnMove(InputValue value)
    {
        direction = value.Get<Vector2>();
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime;
        MovePlayer(deltaTime);
        HandleSpawnBullet(deltaTime);
    }

    private void MovePlayer(float deltaTime)
    {
        transform.position += deltaTime * stats.Speed * (Vector3)direction;
    }

    private void HandleSpawnBullet(float deltaTime)
    {
        shootTimer += deltaTime;
        if (shootTimer >= stats.FireRate)
        {
            shootTimer -= stats.FireRate;
            for (int i = 0; i < stats.BulletStats.Length; i++)
            {
                SpriteRenderer bulletRenderer = new GameObject("bullet").AddComponent<SpriteRenderer>();
                Bullet bullet = bulletRenderer.gameObject.AddComponent<Bullet>();
                bullet.SetStats(stats.BulletStats[i]);
                bulletRenderer.sprite = bulletSprite;
            }
        }
    }
}
