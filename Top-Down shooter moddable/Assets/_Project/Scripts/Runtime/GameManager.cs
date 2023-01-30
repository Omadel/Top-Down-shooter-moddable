using Etienne;
using TMPro;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public PlayerController Player;

    public int Gold;
    public TextMeshProUGUI GoldCount;
    [SerializeField] private Fruit fruitPrefab;

    private void Update()
    {
        GoldCount.text = "Gold : " + Gold.ToString();
    }

    internal Fruit GetFruit(FruitData fruitData)
    {
        Fruit fruit = GameObject.Instantiate(fruitPrefab);
        fruit.SetData(fruitData);
        return fruit;
    }
}
