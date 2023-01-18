using System.Collections;
using UnityEngine;
using Etienne;
using UnityEngine.UI;
using TMPro;



public class GameManager : Singleton<GameManager>
{
    public PlayerController Player;

    public int Gold;
    public TextMeshProUGUI GoldCount;

    private void Update()
    {
        GoldCount.text = "Gold : " + Gold.ToString();
    }
}
