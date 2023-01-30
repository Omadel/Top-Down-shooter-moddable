using UnityEngine;
using UnityEngine.UI;

public class FruitStagesCreator : MonoBehaviour
{
    [SerializeField] private Sprite[] defaultSprites;
    [SerializeField] private Image[] templates;

    public void ResetMod()
    {
        for (int i = 0; i < defaultSprites.Length; i++)
        {
            Image image = templates[i].GetComponentsInChildren<Image>()[1];
            image.sprite = defaultSprites[i];
            image.SetNativeSize();
        }
        for (int i = defaultSprites.Length; i < templates.Length; i++)
        {
            templates[i].gameObject.SetActive(false);
        }
    }
}
