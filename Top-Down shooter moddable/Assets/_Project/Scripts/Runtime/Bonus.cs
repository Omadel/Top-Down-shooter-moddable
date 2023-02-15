using DG.Tweening;
using System;
using System.IO;
using UnityEngine;

public class Bonus : MonoBehaviour
{
    public BonusStats Stats => stats;
    [SerializeField] private BonusStats stats;
    [SerializeField] private Sprite sprite;

    [Serializable]
    public class BonusStats
    {
        public string SpriteName;
        public int Health;
        public float Speed;
        public float FireRate;
        public Bullet.BulletStats[] BulletStats;
        public bool IsPermanent;
        public float Duration;
        public float PathSpeed;
        public Vector3[] Path = new Vector3[] { new Vector2(0, 4), new Vector2(0, -4) };
        public string Ease;
    }

    internal void LoadStats(string path)
    {
        if (!File.Exists(path)) File.WriteAllText(path, JsonUtility.ToJson(stats, true));
        else stats = JsonUtility.FromJson<BonusStats>(File.ReadAllText(path));
        string spritePath = $"{WaveHandler.Instance.path}/{stats.SpriteName}.png";
        if (File.Exists(spritePath)) sprite = Loader.LoadSprite(spritePath);
        GetComponent<SpriteRenderer>().sprite = sprite;

        if (!Enum.TryParse(stats.Ease, out Ease ease))
        {
            ease = Ease.Linear;
            Debug.LogWarning($"Ease {stats.Ease} not parsed !");
        }
        transform.position = stats.Path[0];
        transform.DOPath(stats.Path, stats.PathSpeed).SetSpeedBased(true).SetEase(ease);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.TryGetComponent<Player>(out Player player)) return;
        player.ApplyBonus(stats);
        Destroy(gameObject);
    }
}
