using UnityEngine;

public abstract class Transportable : MonoBehaviour, IInteractable
{

    public virtual bool IsInteractable => isInteractable && !GameManager.Instance.Player.IsTransporting;
    protected new SpriteRenderer renderer;
    protected bool isInteractable = true;


    protected virtual void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        HideInteraction();
    }
    public virtual void ShowInteraction()
    {
        renderer.material.SetColor("_SolidOutline", Color.white);
    }

    public virtual void HideInteraction()
    {
        renderer.material.SetColor("_SolidOutline", Color.clear);
    }

    public virtual void Interact()
    {
        GameManager.Instance.Player.Pickup(this);
        renderer.sortingOrder = 5;
        HideInteraction();
        isInteractable = false;
    }

    public virtual void OnInteractionEnded()
    {
        renderer.sortingOrder = 0;
        isInteractable = true;
    }
}
