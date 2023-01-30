using UnityEngine;

[CreateAssetMenu(menuName = "Farm/Fruit")]
public class FruitData : ScriptableObject
{
    public int GoldValue => goldValue;
    public Sprite Sprite => sprite;
    public Sprite CrateSprite => crateSprite;

    [SerializeField] private int goldValue = 10;
    [SerializeField] private Sprite sprite;
    [SerializeField] Sprite crateSprite;
}