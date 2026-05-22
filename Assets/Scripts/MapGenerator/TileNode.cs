using System.Collections.Generic;
using System.Xml.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class TileNode
{

    private bool positionDirty = true;
    public Vector3 worldPosition;
    public HexaCord hexaCord;
    public bool isWater;

    public TileNode[] neighbors = new TileNode[6];  
    public List<EdgeNode> edges = new List<EdgeNode>(); 
    public List<CornerNode> corners = new List<CornerNode>();

    public Vector3 UpdatePosition()
    {
        if (!positionDirty)
            return worldPosition;
        positionDirty = false;

        float size = MapGenerator.tileOffset + MapGenerator.tileMargin;
        float worldX = size * (1.5f * hexaCord.x);
        float worldZ = size * (Mathf.Sqrt(3f) * (hexaCord.z + hexaCord.x * 0.5f));

        worldPosition = new Vector3(worldX, MapGenerator.worldY, worldZ);

        return worldPosition;
    }
}
