using UnityEngine;
using System.Collections;

public class GridNode : Node
{
    public int gridX;
    public int gridY;

    public GridNode(bool _walkable, Vector3 _worldPos, int _gridX, int _gridY) : base(_walkable, _worldPos)
    {
        gridX = _gridX;
        gridY = _gridY;
    }

}
