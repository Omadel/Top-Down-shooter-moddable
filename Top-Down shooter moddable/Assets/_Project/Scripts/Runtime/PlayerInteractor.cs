using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] float range = 1f;
    [SerializeField] Vector2 offset = Vector3.right;

    IInteractable selectedInteractable;
    new SpriteRenderer renderer;

    private void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {

        var offset = this.offset;
        if (renderer.flipX) offset *= -1;
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position + (Vector3)offset, range, Vector3.up);
        int interactionCount = 0;
        foreach (var hit in hits)
        {
            GameObject go = hit.collider.gameObject;
            if (go == gameObject) continue;
            if (!go.TryGetComponent(out IInteractable interactable)) continue;
            if (!interactable.IsInteractable) continue;
            interactionCount++;
            if (interactable == selectedInteractable) continue;
            selectedInteractable?.HideInteraction();
            selectedInteractable = interactable;
            selectedInteractable.ShowInteraction();
        }
        if (interactionCount == 0)
        {
            selectedInteractable?.HideInteraction();
            selectedInteractable = null;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            selectedInteractable?.Interact();
            selectedInteractable = null;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        using (new UnityEditor.Handles.DrawingScope(Color.green))
        {
            var offset = this.offset;
            if (GetComponent<SpriteRenderer>().flipX) offset *= -1;
            UnityEditor.Handles.DrawWireArc(transform.position + (Vector3)offset, Vector3.back, Vector3.up, 360f, range);
        }
    }
#endif
}
