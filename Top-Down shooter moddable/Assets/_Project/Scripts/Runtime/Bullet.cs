using System;
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
    public class BulletStats
    {
        public int Damage = 10;
        public float Speed = 20f;
        public Vector2 Offset = new Vector2(0f, .15f);
        public float Angle = 0;
    }

    internal void SetStats(BulletStats stats, bool isEnemyFired)
    {
        this.stats = stats;
        transform.SetPositionAndRotation(
            transform.position + (Vector3)stats.Offset,
            Quaternion.AngleAxis(stats.Angle, Vector3.forward));
        this.isEnemyFired = isEnemyFired;
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

