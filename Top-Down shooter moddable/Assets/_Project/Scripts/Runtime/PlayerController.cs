using Etienne.Animator2D;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IHitable, IDestroyable
{
    public int Health => currentHealth;
    public bool IsTransporting => transportable != null;
    public Transportable Transportable => transportable;
    public Fruit TransportedFruit => transportable is Fruit fruit ? fruit : null;


    public float Speed;
    public Etienne.Animator2D.Animator2D Animator;
    public float WeaponRadius;
    public int WeaponDamage;
    public Vector2 WeaponPositionOffset;

    [SerializeField] private int startHealth = 100;
    public SpriteRenderer Renderer;
    private int currentHealth;
    private List<IHitable> hittedHitables = new List<IHitable>();
    private IInteractable currentInteractable;
    private Transportable transportable;


    public void Hit(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Destroy();
        }
    }

    internal void Pickup(Transportable transportable)
    {
        if (this.transportable != null)
        {
            Vector2 offsetWeapon = this.WeaponPositionOffset;
            if (Renderer.flipX) offsetWeapon.x *= -1;
            PutDown(offsetWeapon);
        }
        transportable.transform.SetParent(transform);
        transportable.transform.localPosition = Vector3.up * .15f;
        this.transportable = transportable;
    }

    public void CarryCrate(Crate crate)
    {
        transportable = crate;
        crate.gameObject.SetActive(false);
        Animator.SetState("Carry");
    }

    internal void PutDown(Vector3 position)
    {
        transportable.gameObject.SetActive(true);
        transportable.transform.SetParent(null);
        transportable.transform.position = position;
        transportable.OnInteractionEnded();
        transportable = null;
    }

    internal void Harvest(Soil soil)
    {
        Animator.SetState("Dig", true);
        currentInteractable = soil;
    }

    internal void Sow(Seed seed)
    {
    }

    // Start is called before the first frame update
    private void Start()
    {
        Renderer = GetComponent<SpriteRenderer>();
        Animator = GetComponent<Animator2D>();
        GameManager.Instance.Player = this;
        currentHealth = startHealth;
    }


    // Update is called once per frame
    private void Update()
    {
        Vector2 offsetWeapon = this.WeaponPositionOffset;
        if (Renderer.flipX) offsetWeapon.x *= -1;

        if (Animator.GetState() == "Dig")
        {
            if (Animator.CurrentFrameIndex == 8)
            {
                currentInteractable.OnInteractionEnded();
            }

            return;
        }

        if (Animator.GetState() == "Attack")
        {
            if (Animator.CurrentFrameIndex == 5)
            {
                RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position + (Vector3)offsetWeapon, WeaponRadius, Vector3.forward);
                for (int i = 0; i < hits.Length; i++)
                {
                    RaycastHit2D hit = hits[i];
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
            if (Mathf.Abs(horizontalmvt) > .1f) Animator.FlipX(horizontalmvt < 0);
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

    public void Dig(IInteractable interactable)
    {
        Animator.SetState("Dig", true);
        currentInteractable = interactable;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Vector2 offsetWeapon = this.WeaponPositionOffset;
        if (GetComponent<SpriteRenderer>().flipX) offsetWeapon.x *= -1;
        UnityEditor.Handles.DrawWireArc(transform.position + (Vector3)offsetWeapon, Vector3.forward, Vector3.up, 360, WeaponRadius);
    }
#endif
}
