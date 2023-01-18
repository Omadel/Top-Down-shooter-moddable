using Etienne.Animator2D;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IHitable, IDestroyable
{
    public int Health => currentHealth;

    public float Speed;
    public Etienne.Animator2D.Animator2D Animator;
    public float WeaponRadius;
    public int WeaponDamage;
    public Vector2 WeaponPositionOffset;
    
    [SerializeField] int startHealth = 100;
    public SpriteRenderer Renderer;
    private Vector2 offsetWeapon;

    int currentHealth;
    List<IHitable> hittedHitables = new List<IHitable>();
    
    public void Hit(int amount)
    {
        currentHealth-= amount;
        if(currentHealth<=0)
        {
            Destroy();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Renderer = GetComponent<SpriteRenderer>();
        Animator = GetComponent<Animator2D>();
        GameManager.Instance.Player = this;
        currentHealth = startHealth;
    }

    // Update is called once per frame
    void Update()
    {
        offsetWeapon = this.WeaponPositionOffset;
        if (Renderer.flipX) offsetWeapon.x *= -1;
        if (Animator.GetState() == "Attack")
        {
            if (Animator.CurrentFrameIndex == 5)
            {
                
                
                RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position + (Vector3)offsetWeapon , WeaponRadius, Vector3.forward);
                for (int i = 0; i < hits.Length; i++)
                {
                    var hit = hits[i];
                    if (hit.collider.gameObject == this.gameObject) continue;
                    if (!hit.collider.TryGetComponent<IHitable>(out IHitable hitable)) continue;
                    if (hittedHitables.Contains(hitable)) continue;
                    hitable.Hit(WeaponDamage);
                    hittedHitables.Add(hitable);
                }
                
            }

            return;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Animator.SetState("Attack", true);
            hittedHitables.Clear();
            return;
        }

        float horizontalmvt = Input.GetAxisRaw("Horizontal");
        float verticalmvt = Input.GetAxisRaw("Vertical");
        Vector2 movement = new Vector2(horizontalmvt, verticalmvt).normalized;

        if (movement.sqrMagnitude != 0)
        {
            Animator.SetState("Walking");
            Animator.FlipX(horizontalmvt < 0);
            
            
        }
        else
        {
            Animator.SetState("Idle");
        }
        transform.position += (Vector3)movement * Speed * Time.deltaTime;
        
    }
    public void Destroy()
    {
        Destroy(this.gameObject);
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        UnityEditor.Handles.DrawWireArc(transform.position + (Vector3)offsetWeapon, Vector3.forward, Vector3.up, 360, WeaponRadius);
    }

    
#endif
}
