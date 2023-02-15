using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider mainslider, tempslider;

    private void Start()
    {
        mainslider.value = 1;
        tempslider.value = 1;
    }

    public void SetValue(float value01)
    {
        mainslider.DOKill();
        tempslider.DOKill();
        if (value01 > mainslider.value)
        {
            tempslider.value = value01;
            mainslider.DOValue(value01, .2f).SetDelay(.2f);
            tempslider.fillRect.GetComponent<Image>().color = Color.green;
        }
        else
        {
            mainslider.value = value01;
            tempslider.fillRect.GetComponent<Image>().color = Color.red;
            tempslider.DOValue(value01, .2f).SetDelay(.2f);
        }
    }
}

