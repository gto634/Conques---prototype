// EdgeNode.cs
using System.Collections.Generic;
using UnityEngine;

public class EdgeNode
{
    bool positionDirty = true;
    public Vector3 worldPosition;
    public CornerNode cornerA;
    public CornerNode cornerB;
    public TileNode tileA;          
    public TileNode tileB;    

    public void UpdatePosition()
    {
        if (cornerA == null || cornerB == null || !positionDirty)
            return;
        positionDirty = false;

        worldPosition = (cornerA.worldPosition + cornerB.worldPosition) * 0.5f;
    }
}