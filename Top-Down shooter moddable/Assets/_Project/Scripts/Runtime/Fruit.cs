using UnityEngine;
public class Fruit : Transportable
{
    public FruitData Data => data;
    [SerializeField] private FruitData data;

    public void SetData(FruitData fruit)
    {
        data = fruit;
        if (renderer)
        {
            renderer.sprite = data.Sprite;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = data.Sprite;
        }
    }

    public static implicit operator FruitData(Fruit fruit) => fruit.Data;
}
