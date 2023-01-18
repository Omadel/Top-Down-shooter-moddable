using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour, ISellable
{
    
    public int vegetableCount => currentVegetableCount;
    public int value => currentValue;

    [SerializeField] private int currentVegetableCount = 0;
    [SerializeField] private int currentValue = 0;

    public void Sell(int vegetableCount, int vegetableValue)
    {
        GameManager.Instance.Gold += vegetableValue*vegetableCount;
    }

    public void AddVegetable(int vegetableValue)
    {
        if(currentVegetableCount ==0)
        {
            currentValue = vegetableValue;
        }
        currentVegetableCount++;
        
    }

    public void EmptyBox()
    {
        currentValue = 0;
    }


    // Start is called before the first frame update



}
