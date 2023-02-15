using System;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private int currentHealth;
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
        if (File.Exists(path)) GetComponent<SpriteRenderer>().sprite = Loader.LoadSprite(path);
    }

    private void LoadBulletSprite(string path)
    {
        if (File.Exists(path)) bulletSprite = Loader.LoadSprite(path);
    }

    private void LoadPlayerStats(string path)
    {
        if (!File.Exists(path)) File.WriteAllText(path, JsonUtility.ToJson(stats, true));
        else stats = JsonUtility.FromJson<PlayerStats>(File.ReadAllText(path));
        currentHealth = stats.Health;
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
                bulletRenderer.sprite = bulletSprite;
                bullet.SetStats(stats.BulletStats[i], false);
                bullet.transform.position += transform.position;
                bullet.gameObject.AddComponent<CircleCollider2D>();
            }
        }
    }

    public void Hit(int damage)
    {
        if (!enabled) return;
        currentHealth -= damage;
        healthBar.SetValue(currentHealth / (float)stats.Health);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        WaveHandler.Instance.LooseGame();
        Destroy(gameObject);
    }

    void OnRetry()
    {
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }
}
