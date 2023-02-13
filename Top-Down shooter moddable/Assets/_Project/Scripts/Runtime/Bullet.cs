using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    BulletStats stats;
    new SpriteRenderer renderer;
    private void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
        gameObject.hideFlags = HideFlags.HideAndDontSave;
    }

    [Serializable]
    public class BulletStats
    {
        public float Speed = 20f;
        public Vector2 Offset = new Vector2(0f, .15f);
        public float Angle = 0;
    }

    internal void SetStats(BulletStats stats)
    {
        this.stats = stats;
        transform.SetPositionAndRotation(
            transform.position + (Vector3)stats.Offset,
            Quaternion.AngleAxis(stats.Angle, Vector3.forward));
    }

    private void Update()
    {
        transform.position += Time.deltaTime * stats.Speed * transform.up;
        if (!renderer.isVisible)
        {
            Destroy(gameObject);
        }
    }
}

