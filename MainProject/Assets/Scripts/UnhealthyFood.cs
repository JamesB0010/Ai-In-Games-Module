using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnhealthyFood : MonoBehaviour, I_FoodItem
{
    public bool IsHealthy()
    {
        return false;
    }

    public void Destroy()
    {
        Destroy(this.gameObject);
    }
}
