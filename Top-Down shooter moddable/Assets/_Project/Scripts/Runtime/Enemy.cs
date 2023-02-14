using DG.Tweening;
using System;
using System.IO;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyStats stats;

    [System.Serializable]
    private class EnemyStats
    {
        public string Name = "BaseEnemy";
        public int Health = 20;
        public float Speed = 4f;
        public float FireRate = 1f;
        public int Damage = 10;
        public Bullet.BulletStats[] BulletStats = new Bullet.BulletStats[] { };
        public Vector3[] Path = new Vector3[] { new Vector2(0, 4), new Vector2(0, -4) };
        public string Ease;
    }

    private void Awake()
    {
        string path = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}/My Games/{Application.productName}";
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        LoadEnemyStats($"{path}/{stats.Name}.json");

    }

    private void LoadEnemyStats(string path)
    {
        if (!File.Exists(path)) File.WriteAllText(path, JsonUtility.ToJson(stats, true));
        else stats = JsonUtility.FromJson<EnemyStats>(File.ReadAllText(path));
    }

    private void Start()
    {
        Ease ease = Ease.Linear;
        if (!Enum.TryParse<Ease>(stats.Ease, out ease))
        {
            ease = Ease.Linear;
            Debug.LogWarning($"Ease {stats.Ease} not parsed !");
        }
        transform.DOPath(stats.Path, stats.Speed).SetSpeedBased(true).SetEase(ease);
    }
}
