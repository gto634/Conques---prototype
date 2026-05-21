using System.Collections.Generic;
using UnityEngine;

public class NodeGrid
{
    public Dictionary<HexaCord, MapNode> grid = new Dictionary<HexaCord, MapNode>();
    public HashSet<HexaCord> freeCord = new HashSet<HexaCord>();

    public void Reset()
    {
        freeCord.Clear();
        grid.Clear();
        freeCord.Add(new HexaCord(0, 0, 0));
    }

    public HashSet<HexaCord> GetFreeCords()
    {
        return freeCord; 
    }
    public HexaCord GetRandomFreeCord()
    {
        int index = Random.Range(0, freeCord.Count - 1);
        int i = 0;

        foreach (var c in freeCord)
        {
            if (i++ == index) return c;
        }

        throw new System.Exception("No free cords");
    }

    public MapNode TryGetNode(HexaCord cord)
    {
        grid.TryGetValue(cord, out MapNode node);
        return node;
    }

    public MapNode CreateNewNode(HexaCord cord)
    {
        MapNode node = new MapNode();
        node.hexaCord = cord;
        grid[cord] = node;

        freeCord.Remove(cord);

        for (int i = 0; i < HexaCord.directions.Length; i++)
        {
            HexaCord dir = HexaCord.directions[i];
            HexaCord neighborCord = cord + dir;

            if (grid.TryGetValue(neighborCord, out MapNode neighbor))
            {
                node.neighbors[i] = neighbor;
                neighbor.neighbors[(i + 3) % 6] = node;
            }
            else
            {
                freeCord.Add(neighborCord);
            }
        }

        return node;
    }
}
