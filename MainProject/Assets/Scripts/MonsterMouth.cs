using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMouth : MonoBehaviour
{
    public event Action<I_FoodItem> foodInMouth;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out I_FoodItem foodItem))
        {
            this.foodInMouth?.Invoke(foodItem);
        }
    }
}
