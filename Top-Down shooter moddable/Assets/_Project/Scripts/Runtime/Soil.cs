using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Soil : MonoBehaviour, IInteractable
{
    public bool IsInteractable => isInteractable;

    [SerializeField] Sprite normalSprite, dugSprite;

    bool isInteractable = true;
    new SpriteRenderer renderer;

    private void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        renderer.sprite = normalSprite;
    }

    private void OnValidate()
    {
        GetComponent<SpriteRenderer>().sprite = normalSprite;
    }

    public void ShowInteraction()
    {
        Debug.Log("Show Interaction");
    }

    public void HideInteraction()
    {
        Debug.Log("Hide Interaction");
    }

    public void Interact()
    {
        Debug.Log("Interact");
        HideInteraction();
        isInteractable = false;
        renderer.sprite = dugSprite;
    }
}
