using Etienne.Animator2D;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float Speed;
    public float RandomRange;
    public float PlayerRange;
    public float AttackRange = .5f;
    public Etienne.Animator2D.Animator2D Animator;
    public bool IsMoving;
    public Vector2 TargetPosition;
    public Vector2 StartPosition;
    public enum AIState
    {
        Idle, Move, Attack, MoveToAttack
    }
    public AIState State;

    // Start is called before the first frame update
    void Start()
    {
        Animator = GetComponent<Animator2D>();
        StartPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Animator.FlipX(TargetPosition.x < transform.position.x);
        if (State == AIState.Attack)
        {
            TargetPosition = GameManager.Instance.Player.transform.position;
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
    }
#endif 
}
    