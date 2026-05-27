// EdgeNode.cs
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EdgeNode
{
    bool positionDirty = true;
    public Vector3 worldPosition;
    public CornerNode cornerA;
    public CornerNode cornerB;
    public TileNode tileA;          
    public TileNode tileB;

    public bool hasRoad;

    public void UpdatePosition()
    {
        if (cornerA == null || cornerB == null || !positionDirty)
            return;
        positionDirty = false;

        worldPosition = (cornerA.worldPosition + cornerB.worldPosition) * 0.5f;
    }

    public bool CanBuild()
    {
        if ((!cornerA.hasBuild && !cornerB.hasBuild) || hasRoad)
            return false;

        bool isWaterSurrounded = false;
        if ((tileA == null || tileA.isWater) && (tileB == null || tileB.isWater))
            isWaterSurrounded = true;

        return !isWaterSurrounded;
    }

    public bool IsCoastal()
    {
        return ((tileA == null || tileA.isWater) ^ (tileB.isWater || tileB.isWater)); // ^ is xor
    }

    public bool HasNoCornerPorts()
    {
        return (!cornerA.hasPort && !cornerB.hasPort);
    }

    public bool HasNoCornerNearPorts()
    {
        return !cornerA.HasNeighborPorts() && !cornerB.HasNeighborPorts();
    }

    public void DrawGizmo()
    {
        if (hasRoad)
            Gizmos.color = Color.yellow;
        else if (CanBuild())
            Gizmos.color = Color.green;
        else
            Gizmos.color = Color.red;

        Gizmos.DrawSphere(worldPosition, 0.1f);
    }
}