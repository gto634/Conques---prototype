// CornerNode.cs
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class CornerNode
{
    bool positionDirty = true;
    public Vector3 worldPosition;
    public List<EdgeNode> edges = new List<EdgeNode>();
    public List<TileNode> adjacentTiles = new List<TileNode>();

    public bool hasBuild = false; // change later
    public bool hasPort = false; // change later

    public void UpdatePosition()
    {
        if (!positionDirty || adjacentTiles.Count != 3)
            return;
        positionDirty = false;

        Vector3 averagePosition = Vector3.zero;
        for (int i = 0; i < adjacentTiles.Count; i++)
        {
            TileNode tile = adjacentTiles[i];
            averagePosition += tile.worldPosition;
        }
        averagePosition /= adjacentTiles.Count;

        worldPosition = averagePosition;
    }

    public bool CanBuild()
    {
        if (hasBuild) return false;

        for (int i = 0; i < adjacentTiles.Count; i++)
        {
            TileNode tile = adjacentTiles[i];
            if (!tile.isWater)
                return true;
        }

        return false;
    }
    public void DrawGizmo()
    {

        if (hasBuild)
            Gizmos.color = Color.yellow;
        else if (CanBuild())
            Gizmos.color = Color.green;
        else
            Gizmos.color = Color.red;

        Gizmos.DrawSphere(worldPosition, 0.1f);
    }

    public int GetTilesOdds()
    {
        int odds = 0;
        for (int i = 0; i < adjacentTiles.Count; i++)
        {
            TileNode tile = adjacentTiles[i];
            if (tile.tileValue != -1)
            {
                if (MapNumbersEntry.odds.TryGetValue(tile.tileValue, out int odd))
                    odds += odd;
            }
                
        }
        return odds;
    }

    public int GetTileMaxOdd()
    {
        int max = 0;
        for (int i = 0; i < adjacentTiles.Count; i++)
        {
            TileNode tile = adjacentTiles[i];
            if (tile.tileValue != -1)
            {
                if (MapNumbersEntry.odds.TryGetValue(tile.tileValue, out int odd))
                    max = Mathf.Max(max, odd);
            }

        }
        return max;
    }

    public bool HasNeighborPorts()
    {
        for (int i = 0; i < edges.Count; i++)
        {
            EdgeNode edge = edges[i];
            if(!edge.HasNoCornerPorts())
                return true;
        }
        return false;
    }
}