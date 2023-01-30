using UnityEngine;

public class Crate : Transportable, ISellable
{
    public override bool IsInteractable => isInteractable;

    public int value => goldValue;

    public int vegetableCount => fruitAmount;

    [SerializeField] private int fruitAmount = 0;
    [SerializeField] private int goldValue;
    [SerializeField] private FruitData fruitData;
    [SerializeField] private TMPro.TextMeshProUGUI countTextMesh;

    private SpriteRenderer[] renderers;

    protected override void Start()
    {
        base.Start();
        renderers = GetComponentsInChildren<SpriteRenderer>();
        renderers[1].sprite = null;
        countTextMesh.enabled = false;
    }

    public override void Interact()
    {
        PlayerController player = GameManager.Instance.Player;
        Fruit fruit = player.TransportedFruit;
        if (player.IsTransporting && fruit != null && (fruitData == null || fruit.Data == fruitData))
        {
            player.PutDown(transform.position);
            fruitData = fruit;
            fruitAmount++;

            countTextMesh.enabled = fruitAmount > 1;
            countTextMesh.text = $"x{fruitAmount}";

            FruitData data = fruit.Data;
            renderers[1].sprite = data.CrateSprite;
            goldValue = data.GoldValue;

            GameObject.Destroy(fruit.gameObject);
        }
        else
        {
            base.Interact();
            int baseOrder = renderer.sortingOrder;
            for (int i = 1; i < renderers.Length; i++)
            {
                renderers[i].sortingOrder = baseOrder + i;
            }
        }
    }

    public override void OnInteractionEnded()
    {
        base.OnInteractionEnded();

        int baseOrder = renderer.sortingOrder;
        for (int i = 1; i < renderers.Length; i++)
        {
            renderers[i].sortingOrder = baseOrder + i;
        }
    }

    public void Sell(int vegetableAmount, int vegetableValue)
    {
        GameManager.Instance.Gold += vegetableValue * vegetableCount;
    }

    public void EmptyBox()
    {
        goldValue = 0;
    }
}
