// CornerNode.cs
using System.Collections.Generic;
using UnityEngine;

public class CornerNode
{
    bool positionDirty = true;
    public Vector3 worldPosition;
    public List<EdgeNode> edges = new List<EdgeNode>();
    public List<TileNode> adjacentTiles = new List<TileNode>();

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
}