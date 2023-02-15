using DG.Tweening;
using System;
using System.IO;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyStats Stats => stats;

    public event System.Action OnDie;

    [SerializeField] private HealthBar healthBar;
    [SerializeField] private int currentHealth;
    [SerializeField] private EnemyStats stats;
    [SerializeField] private Sprite bulletSprite;
    private float shootTimer;

    [System.Serializable]
    public class EnemyStats
    {
        public string Name = "BaseEnemy";
        public int Health = 20;
        public float Speed = 4f;
        public float FireRate = 1f;
        public int KamikazeDamage = 50;
        public Bullet.BulletStats[] BulletStats = new Bullet.BulletStats[] { };
        public Vector3[] Path = new Vector3[] { new Vector2(0, 4), new Vector2(0, -4) };
        public string Ease;
    }

    public void LoadEnemyStats(string path)
    {
        if (!File.Exists(path)) File.WriteAllText(path, JsonUtility.ToJson(stats, true));
        else stats = JsonUtility.FromJson<EnemyStats>(File.ReadAllText(path));
        currentHealth = stats.Health;
        string spritePath = $"{WaveHandler.Instance.path}/{stats.Name}.png";
        if (stats.Name!="" && File.Exists(spritePath))
        {
            GetComponent<SpriteRenderer>().sprite = Loader.LoadSprite(spritePath);
        }
        transform.position = stats.Path[0];
    }

    private void Start()
    {
        if (!Enum.TryParse(stats.Ease, out Ease ease))
        {
            ease = Ease.Linear;
            Debug.LogWarning($"Ease {stats.Ease} not parsed !");
        }
        transform.DOPath(stats.Path, stats.Speed).SetSpeedBased(true).SetEase(ease).OnComplete(() => Destroy(gameObject));
    }

    internal void Hit(int damage)
    {
        currentHealth -= damage;
        healthBar.SetValue(currentHealth / (float)stats.Health);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        transform.DOKill();
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        OnDie?.Invoke();
    }

    private void Update()
    {
        HandleSpawnBullet(Time.deltaTime);
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
                bullet.SetStats(stats.BulletStats[i], true);
                bullet.transform.position += transform.position;
                bullet.gameObject.AddComponent<CircleCollider2D>();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.TryGetComponent<Player>(out Player player))
        {
            player.Hit(stats.KamikazeDamage);
            GameObject.Destroy(gameObject);
        }
    }
}
