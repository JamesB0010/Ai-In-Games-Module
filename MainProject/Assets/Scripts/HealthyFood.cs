using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthyFood : MonoBehaviour, I_FoodItem
{
    public bool IsHealthy()
    {
        return true;
    }

    public void Destroy()
    {
        Destroy(this.gameObject);
    }
}
