using Etienne;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Soil : MonoBehaviour, IInteractable
{
    private PlayerController player => GameManager.Instance.Player;
    public bool IsInteractable
    {
        get
        {
            if (player.Transportable is Seed) return isDug;
            if (isInteractable && !player.IsTransporting) return true;
            return false;
        }
    }

    [SerializeField] private Sprite normalSprite, dugSprite;
    [SerializeField] private SpriteRenderer fruitRenderer;
    [SerializeField, ReadOnly] private FruitData fruit;

    private bool isDug = false;
    private bool isSew = false;
    private bool isInteractable = true;
    private new SpriteRenderer renderer;
    private Sprite[] stages;
    private float duration;

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
        if (isSew)
        {
            player.Harvest(this);
        }
        else if (player.IsTransporting)
        {
            Seed seed = player.Transportable as Seed;
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

    private void Sow(SeedData seed)
    {
        isSew = true;
        fruit = seed.Fruit;
        fruitRenderer.sprite = seed.GrowingStages[0];
        stages = seed.GrowingStages;
        duration = seed.GrowingDuration;
        Timer timer = Timer.Create(seed.GrowingDuration).OnUpdate(UpdateFruit).OnComplete(CompleteFruit);
        timer.Restart();
    }

    private void CompleteFruit()
    {
        renderer.sprite = normalSprite;
        isInteractable = true;
    }

    private void UpdateFruit(float timerValue)
    {
        int index = Mathf.FloorToInt((timerValue / duration) * stages.Length);
        fruitRenderer.sprite = stages[Mathf.Min(index, stages.Length - 1)];
    }

    public void OnInteractionEnded()
    {
        if (!isDug)
        {
            isDug = true;
            renderer.sprite = dugSprite;
        }
        else if (isSew)
        {
            Harvest();
        }
    }

    private void Harvest()
    {
        player.Pickup(GameManager.Instance.GetFruit(this.fruit));
        isSew = false;
        isDug = false;
        isInteractable = true;
        fruitRenderer.sprite = null;
        renderer.sprite = normalSprite;
    }
}
