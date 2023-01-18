using Etienne.Animator2D;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour, IHitable, IDestroyable
{
    public int Health => currentHealth;
    int currentHealth;
    [SerializeField] int startHealth = 10;
    public float Speed;
    public float RandomRange;
    public float PlayerRange;
    public float AttackRange = .5f;
    public Etienne.Animator2D.Animator2D Animator;
    public bool IsMoving;
    public Vector2 TargetPosition;
    public Vector2 StartPosition;

    public float WeaponRadius;
    public int WeaponDamage;
    public Vector2 WeaponPositionOffset;
    
    List<IHitable> hittedHitables = new List<IHitable>();
    public SpriteRenderer Renderer;
    private Vector2 offset;
    public enum AIState
    {
        Idle, Move, Attack, MoveToAttack
    }
    public AIState State;

    

    // Start is called before the first frame update
    void Start()
    {
        Renderer = GetComponent<SpriteRenderer>();
        Animator = GetComponent<Animator2D>();
        StartPosition = transform.position;
        currentHealth = startHealth;
    }

    // Update is called once per frame
    void Update()
    {
        offset = this.WeaponPositionOffset;
        if (Renderer.flipX) offset.x *= -1;
        Animator.FlipX(TargetPosition.x < transform.position.x);
        if (State == AIState.Attack)
        {
            if(GameManager.Instance.Player != null)
            TargetPosition = GameManager.Instance.Player.transform.position;

            if(Animator.CurrentFrameIndex == 1)
            {
                hittedHitables.Clear();
            }

            if (Animator.CurrentFrameIndex == 5)
            {
                RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position + (Vector3)offset, WeaponRadius, Vector3.up);
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

            if (DistanceToTarget() > AttackRange)
            {
                State = AIState.MoveToAttack;
            }
        }
        if (State != AIState.MoveToAttack && State != AIState.Attack && Vector3.Distance(transform.position, GameManager.Instance.Player.transform.position) < PlayerRange)
        {
            State = AIState.MoveToAttack;
        }
        if (State == AIState.MoveToAttack)
        {
            TargetPosition = GameManager.Instance.Player.transform.position;
            MoveTowardTargetPosition();
            if (DistanceToTarget() < AttackRange)
            {
                Attack();
            }
        }
        if (State == AIState.Move)
        {
            MoveTowardTargetPosition();
            if (DistanceToTarget() < .5f)
            {
                State = AIState.Idle;
                Animator.SetState("Idle");
            }
        }
        else if (State == AIState.Idle)
        {
            TargetPosition = StartPosition + Random.insideUnitCircle * RandomRange;
            State = AIState.Move;
        }
    }

    private float DistanceToTarget()
    {
        return Vector3.Distance(transform.position, TargetPosition);
    }

    private void MoveTowardTargetPosition()
    {
        Animator.SetState("Move");
        transform.position = Vector2.MoveTowards(transform.position, TargetPosition, Speed * Time.deltaTime);
    }

    private void Attack()
    {
        State = AIState.Attack;
        
        Animator.SetState("Attack", true);
        
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Vector2 position = Application.isPlaying ? StartPosition : transform.position;
        UnityEditor.Handles.DrawWireArc(position, Vector3.forward, Vector3.up, 360, RandomRange);
        UnityEditor.Handles.DrawWireArc(transform.position, Vector3.forward, Vector3.up, 360, PlayerRange);
        UnityEditor.Handles.DrawWireArc(transform.position, Vector3.forward, Vector3.up, 360, AttackRange);
        UnityEditor.Handles.DrawWireArc(transform.position + (Vector3)offset, Vector3.forward, Vector3.up, 360, WeaponRadius);
    }
#endif

    public void Hit(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Destroy();
        }
    }

    public void Destroy()
    {
        Destroy(this.gameObject);
    }
}
    