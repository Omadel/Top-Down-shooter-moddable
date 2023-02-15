using DG.Tweening;
using System;
using System.IO;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyStats Stats=>stats;

    public event System.Action OnDie;

    [SerializeField] int currentHealth;
    [SerializeField] private EnemyStats stats;
    [SerializeField] Sprite bulletSprite;

    float shootTimer;

    [System.Serializable]
    public class EnemyStats
    {
        public string Name = "BaseEnemy";
        public int Health = 20;
        public float Speed = 4f;
        public float FireRate = 1f;
        public Bullet.BulletStats[] BulletStats = new Bullet.BulletStats[] { };
        public Vector3[] Path = new Vector3[] { new Vector2(0, 4), new Vector2(0, -4) };
        public string Ease;
    }

    public void LoadEnemyStats(string path)
    {
        if (!File.Exists(path)) File.WriteAllText(path, JsonUtility.ToJson(stats, true));
        else stats = JsonUtility.FromJson<EnemyStats>(File.ReadAllText(path));
        currentHealth = stats.Health;
    }

    private void Start()
    {
        Ease ease;
        if (!Enum.TryParse(stats.Ease, out ease))
        {
            ease = Ease.Linear;
            Debug.LogWarning($"Ease {stats.Ease} not parsed !");
        }
        transform.DOPath(stats.Path, stats.Speed).SetSpeedBased(true).SetEase(ease).OnComplete(()=>Destroy(gameObject));
    }

    internal void Hit(int damage)
    {
       currentHealth -= damage;
        if (currentHealth<=0)
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
                bullet.SetStats(stats.BulletStats[i], true);
                bulletRenderer.sprite = bulletSprite;
                bullet.transform.position += transform.position;
                bullet.gameObject.AddComponent<CircleCollider2D>();
            }
        }
    }
}
