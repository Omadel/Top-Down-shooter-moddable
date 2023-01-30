using UnityEngine;

[CreateAssetMenu(menuName ="Farm/Seed")]
public class SeedData:ScriptableObject
{
    public float GrowingDuration => growingDuration;
    public Sprite[] GrowingStages => growingStages;
    public FruitData Fruit => fruit;

    [SerializeField] private float growingDuration = 2f;
    [SerializeField] private Sprite[] growingStages;
    [SerializeField] private FruitData fruit;
}