using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : Transportable
{
    public float GrowingDuration => growingDuration;
    public Sprite[] GrowingStages => growingStages;

    [SerializeField] float growingDuration = 2f;
    [SerializeField] Sprite[] growingStages;
}
