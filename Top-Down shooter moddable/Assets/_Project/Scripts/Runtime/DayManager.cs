using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Etienne;
using UnityEngine.UI;
using TMPro;

public class DayManager : Singleton<DayManager>
{
    public Slider SliderDay;
    public float DayTime;
    public float TimeSpeed = 1;
    private float timer;
    public int DayCount;
    public TextMeshProUGUI DayCountText;
    public bool IsDay;
    public bool DeliveryDone = false;
    public DeliveryZone delivery;
    void Start()
    {
        timer = DayTime * 0.3f;
    }

    // Update is called once per frame
    void Update()
    {
        

        timer += TimeSpeed * Time.deltaTime;
       
        SliderDay.value = timer / DayTime;
        if (SliderDay.value == 1)
        {
            timer -= DayTime;
            NewDay();
            return;
        }
        if(SliderDay.value>0.5f && DeliveryDone ==false)
        {
            Debug.Log("Delivery");
            MidDay();

        }
        if (SliderDay.value > 0.25f && SliderDay.value < 0.75f)
        {
            IsDay = true;
            DayCountText.text = "Day " + DayCount;
        }
        else
        {
            IsDay = false;
            DayCountText.text = "Night " + DayCount;
        }
    }
    

    void NewDay()
    {
        Debug.Log("New day");
        DeliveryDone = false;
        DayCount++;

    }
    void MidDay()
    {
        DeliveryDone = true;
        delivery.SellVegetablesBoxes();
    }
}
