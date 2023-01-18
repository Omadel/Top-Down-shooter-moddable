using Etienne.Animator2D;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Speed;
    public Etienne.Animator2D.Animator2D Animator;
    // Start is called before the first frame update
    void Start()
    {
        Animator = GetComponent<Animator2D>();
        GameManager.Instance.Player = this;
    }

    // Update is called once per frame
    void Update()
    {

        if (Animator.GetState() == "Attack")
        {
            if (Animator.CurrentFrameIndex == 5)
            {
                Debug.Log("Attack");
            }

            return;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Animator.SetState("Attack", true);
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
}
