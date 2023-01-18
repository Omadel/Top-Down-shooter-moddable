using Etienne;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Soil : MonoBehaviour, IInteractable
{
    PlayerController player => GameManager.Instance.Player;
    public bool IsInteractable
    {
        get
        {
            if (!isInteractable) return false;
            if (!player.IsTransporting) return true;
            if (player.Transportable is Seed) return true;
            return false;
        }
    }

    [SerializeField] Sprite normalSprite, dugSprite;
    [SerializeField] SpriteRenderer fruitRenderer;

    bool isDug = false;
    bool isInteractable = true;
    new SpriteRenderer renderer;

    Sprite[] stages;
    float duration;

    private void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        renderer.sprite = normalSprite;
        HideInteraction();
        fruitRenderer.sprite = null;
    }

    private void OnValidate()
    {
        GetComponent<SpriteRenderer>().sprite = normalSprite;
    }

    public void ShowInteraction()
    {
        renderer.material.SetColor("_SolidOutline", Color.white);
    }

    public void HideInteraction()
    {
        renderer.material.SetColor("_SolidOutline", Color.clear);
    }

    public void Interact()
    {
        isInteractable = false;
        PlayerController player = GameManager.Instance.Player;
        if (player.IsTransporting)
        {
            var seed = player.Transportable as Seed;
            player.PutDown(Vector3.zero);
            player.Sow(seed);
            Sow(seed);
            GameObject.Destroy(seed.gameObject);
        }
        else
        {
            player.Dig(this);
        }
        HideInteraction();
    }

    private void Sow(Seed seed)
    {
        fruitRenderer.sprite = seed.GrowingStages[0];
        stages = seed.GrowingStages;
        duration = seed.GrowingDuration;
        var timer = Timer.Create(seed.GrowingDuration).OnUpdate(UpdateFruit).OnComplete(CompleteFruit);
        timer.Restart();
    }

    private void CompleteFruit()
    {
        renderer.sprite = normalSprite;
        isInteractable = true;
        isDug = false;
        //todo spawn fuit
    }

    private void UpdateFruit(float timerValue)
    {
        int index = Mathf.FloorToInt((timerValue / duration) * stages.Length);
        fruitRenderer.sprite = stages[Mathf.Min(index, stages.Length - 1)];
    }

    public void OnInteractionEnded()
    {
        isDug = true;
        renderer.sprite = dugSprite;
    }
}
