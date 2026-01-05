using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private List<GameObject> items = new();

    public void AddItem(GameObject i)
    {
        this.items.Add(i);
    }

    public GameObject FindItemWithTag(string tag)
    {
        foreach (var item in this.items)
        {
            if (item.CompareTag(tag))
                return item;
        }

        return null;
    }

    public void RemoveItem(GameObject i)
    {
        items.Remove(i);
    }
}
