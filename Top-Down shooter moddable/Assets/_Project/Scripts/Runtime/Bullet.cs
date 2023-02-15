using System;
using System.IO;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private BulletStats stats;
    private new SpriteRenderer renderer;
    private bool isEnemyFired;
    private void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
        // gameObject.hideFlags = HideFlags.HideAndDontSave
    }

    [Serializable]
    public struct BulletStats
    {
        public string SpriteName;
        public int Damage;
        public float Speed;
        public Vector2 Offset;
        public float Angle;

    }

    internal void SetStats(BulletStats stats, bool isEnemyFired)
    {
        this.stats = stats;
        transform.SetPositionAndRotation(
            transform.position + (Vector3)stats.Offset,
            Quaternion.AngleAxis(stats.Angle, Vector3.forward));
        this.isEnemyFired = isEnemyFired;
        string spritePath = $"{WaveHandler.Instance.path}/{stats.SpriteName}.png";
        if (File.Exists(spritePath)) renderer.sprite = Loader.LoadSprite(spritePath);
        transform.localScale = Vector3.one*.5f;
    }

    private void Update()
    {
        transform.position += Time.deltaTime * stats.Speed * transform.up;
        if (!renderer.isVisible)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isEnemyFired)
        {
            if (collision.gameObject.TryGetComponent<Player>(out Player player))
            {
                player.Hit(stats.Damage);
                Destroy(gameObject);
            }
        }
        else
        {
            if (collision.gameObject.TryGetComponent<Enemy>(out Enemy enemy))
            {
                enemy.Hit(stats.Damage);
                Destroy(gameObject);
            }
        }
    }
}

