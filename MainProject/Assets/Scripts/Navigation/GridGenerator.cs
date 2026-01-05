using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GridGenerator : ScriptableObject
{
    public LayerMask unwalkableMask;
    
    public float nodeRadius;
    
    public abstract NavigationNodeCollection GenerateGrid(GameObject terrain);

}
