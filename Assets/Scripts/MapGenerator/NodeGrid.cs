using System.Collections.Generic;
using UnityEngine;

public class NodeGrid
{
    public Dictionary<HexaCord, TileNode> tileGrid = new Dictionary<HexaCord, TileNode>();
    public HashSet<HexaCord> freeTileCord = new HashSet<HexaCord>();
    public Dictionary<CornerKey, CornerNode> cornerMap = new Dictionary<CornerKey, CornerNode>();
    public Dictionary<EdgeKey, EdgeNode> edgeMap = new Dictionary<EdgeKey, EdgeNode>();
    public List<CornerNode> allCorners = new List<CornerNode>();
    public List<EdgeNode> allEdges = new List<EdgeNode>();
    public void Reset()
    {
        freeTileCord.Clear();
        tileGrid.Clear();
        freeTileCord.Add(new HexaCord(0, 0, 0));
    }

    public HashSet<HexaCord> GetFreeTileCords()
    {
        return freeTileCord; 
    }
    public HexaCord GetRandomFreeTileCord()
    {
        int index = Random.Range(0, freeTileCord.Count - 1);
        int i = 0;

        foreach (var c in freeTileCord)
        {
            if (i++ == index) return c;
        }

        throw new System.Exception("No free tile cords");
    }

    public TileNode CreateNewNode(HexaCord cord)
    {
        TileNode node = new TileNode();
        node.hexaCord = cord;
        tileGrid[cord] = node;
        freeTileCord.Remove(cord);

        for (int i = 0; i < HexaCord.directions.Length; i++)
        {
            HexaCord dir = HexaCord.directions[i];
            HexaCord neighborCord = cord + dir;
            if (tileGrid.TryGetValue(neighborCord, out TileNode neighbor))
            {
                node.neighbors[i] = neighbor;
                neighbor.neighbors[(i + 3) % 6] = node;
            }
            else
            {
                freeTileCord.Add(neighborCord);
            }
        }

        node.UpdatePosition();

        AddTileToGraph(node);
        return node;
    }

    private void AddTileToGraph(TileNode tile)
    {
        Vector3[] cornerWorldPos = new Vector3[6];
        for (int i = 0; i < 6; i++)
        {
            cornerWorldPos[i] = GetHexCorner(tile.worldPosition, i);
        }

        List<CornerNode> tileCorners = new List<CornerNode>(6);

        for (int i = 0; i < 6; i++)
        {
            CornerKey key = new CornerKey(cornerWorldPos[i]);
            if (!cornerMap.TryGetValue(key, out CornerNode corner))
            {
                corner = new CornerNode();
                corner.worldPosition = cornerWorldPos[i];
                cornerMap[key] = corner;
                allCorners.Add(corner);
            }
            if (!corner.adjacentTiles.Contains(tile))
                corner.adjacentTiles.Add(tile);
            tileCorners.Add(corner);
            tile.corners.Add(corner);
        }

        for (int i = 0; i < 6; i++)
        {
            CornerNode a = tileCorners[i];
            CornerNode b = tileCorners[(i + 1) % 6];
            EdgeKey edgeKey = new EdgeKey(new CornerKey(a.worldPosition), new CornerKey(b.worldPosition));
            if (!edgeMap.TryGetValue(edgeKey, out EdgeNode edge))
            {
                edge = new EdgeNode();
                edge.cornerA = a;
                edge.cornerB = b;
                edge.worldPosition = (a.worldPosition + b.worldPosition) * 0.5f;
                edgeMap[edgeKey] = edge;
                allEdges.Add(edge);
            }

            Vector3 edgeDir = (b.worldPosition - a.worldPosition).normalized;
            Vector3 toCenter = tile.worldPosition - edge.worldPosition;
            float cross = Vector3.Cross(edgeDir, toCenter).y;
            if (cross > 0)
                edge.tileA = tile;
            else
                edge.tileB = tile;

            if (!tile.edges.Contains(edge))
                tile.edges.Add(edge);
            if (!a.edges.Contains(edge))
                a.edges.Add(edge);
            if (!b.edges.Contains(edge))
                b.edges.Add(edge);
        }
    }

    private Vector3 GetHexCorner(Vector3 center, int cornerIndex)
    {
        float radius = MapGenerator.tileOffset + MapGenerator.tileMargin;
        float angleDeg = 60f * cornerIndex;
        float angleRad = Mathf.Deg2Rad * angleDeg;
        float x = center.x + radius * Mathf.Cos(angleRad);
        float z = center.z + radius * Mathf.Sin(angleRad);
        return new Vector3(x, center.y, z);
    }
}
