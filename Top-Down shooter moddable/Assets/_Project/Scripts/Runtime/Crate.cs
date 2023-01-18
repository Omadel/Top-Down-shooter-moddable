using UnityEngine;

public class Crate : Transportable, ISellable
{
    public override bool IsInteractable => isInteractable;

    public int value => v;

    public int vegetableCount => fruitAmount;

    [SerializeField] int fruitAmount = 0;
    [SerializeField] int v;
    [SerializeField] Fruit fruit;

    SpriteRenderer[] renderers;

    protected override void Start()
    {
        base.Start();
        renderers = GetComponentsInChildren<SpriteRenderer>();
        renderers[1].sprite = null;
    }

    public override void Interact()
    {
        PlayerController player = GameManager.Instance.Player;
        if (player.IsTransporting && player.Transportable is Fruit fruit)
        {
            player.PutDown(transform.position);
            renderers[1].sprite = fruit.FruitSprite;
            this.fruit = fruit;
            v = fruit.Value;
            fruitAmount++;
            GameObject.Destroy(fruit.gameObject);
        }
        else
        {
            base.Interact();
            var baseOrder = renderer.sortingOrder;
            for (int i = 1; i < renderers.Length; i++)
            {
                renderers[i].sortingOrder = baseOrder + i;
            }
        }
    }

    public override void OnInteractionEnded()
    {
        base.OnInteractionEnded();

        var baseOrder = renderer.sortingOrder;
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
        v = 0;
    }
}
