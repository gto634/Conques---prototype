using System.Collections.Generic;
using System.Xml.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class MapNode
{
    public HexaCord hexaCord;
    public MapNode[] neighbors = new MapNode[6];
}
