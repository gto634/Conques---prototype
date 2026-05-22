using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Audio;

public class MapGenerator : MonoBehaviour
{
    public GameObject map;
    public NodeGrid nodeGrid;

    public MapGenerationMode generationMode;

    public List<TileEntry> tiles;
    public GameObject waterTile;

    public List<PortEntry> ports;

    public static float tileOffset;
    public static float tileMargin;
    public static float worldY;

    private static readonly Vector3[] tileRotations =
    {
        new Vector3(0,30,0),
        new Vector3(0,90,0),
        new Vector3(0,150,0),
        new Vector3(0,210,0),
        new Vector3(0,270,0),
        new Vector3(0,330,0)
    };

    void Awake()
    {
        map = new GameObject("Map");
        map.transform.position = transform.position;

        nodeGrid = new NodeGrid();

        if (tiles.Count == 0)
            return;

        Renderer renderer = tiles[0].prefab.GetComponentInChildren<Renderer>();
        Vector3 size = renderer.bounds.size;
        MapGenerator.tileOffset = Mathf.Max(size.x,size.z) * 0.5f;
        MapGenerator.worldY = transform.position.y;
        tileMargin = 0.1f;
    }

    void Update()
    {
        if (nodeGrid == null || nodeGrid.tileGrid == null)
            return;

        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            generationMode.Generate(this);
            nodeGrid.allCorners.ForEach((corner) => { corner.UpdatePosition(); });
            nodeGrid.allEdges.ForEach((edge) => { edge.UpdatePosition(); });
        } 
    }

    void OnDrawGizmos()
    {
        if (nodeGrid == null || nodeGrid.tileGrid == null)
            return;

        Gizmos.color = Color.red;

        foreach (TileNode node in nodeGrid.tileGrid.Values)
        {
            for (int i = 0; i < node.corners.Count; i++)
            {
                CornerNode corner = node.corners[i];
                Gizmos.DrawSphere(corner.worldPosition, 0.15f);
            }

            for (int i = 0; i < node.edges.Count; i++)
            {
                EdgeNode edge = node.edges[i];
                Gizmos.DrawSphere(edge.worldPosition, 0.15f);
            }
        }
    }

    public void InstanciateTile(HexaCord cord, GameObject prefab)
    {
        TileNode node = nodeGrid.CreateNewNode(cord);
        
        Vector3 position = node.worldPosition;

        GameObject firstTile = Instantiate(prefab, position, Quaternion.identity, map.transform);
        ApplyTileRotation(firstTile);
    }

    public void InstanciateWaterBorder()
    {
        List<HexaCord> freeCords = nodeGrid.GetFreeTileCords().ToList<HexaCord>();
        for (int i = 0; i < freeCords.Count; i++)
        {
            HexaCord cord = freeCords[i];
            InstanciateTile(cord, waterTile);
        }
    }
    public void ResetMap()
    {
        Destroy(map);
        map = new GameObject("Map");

        nodeGrid.Reset();
    }

    public List<int> GetTilePool(float mult)
    {
        List<int> tilePool = new List<int>();

        for (int prefabIndex = 0; prefabIndex < tiles.Count; prefabIndex++)
        {
            int poolAmount = (int)Mathf.Ceil(tiles[prefabIndex].amount * mult); // why does ceil returns float...

            for (int i = 0; i < poolAmount; i++)
            {
                tilePool.Add(prefabIndex);
            }
        }

        List<int> shuffledTilePool = tilePool.OrderBy(x => Random.value).ToList();
        return shuffledTilePool;
    }

    public void ApplyTileRotation(GameObject tile)
    {
        tile.transform.Rotate(tileRotations[Random.Range(0, tileRotations.Length - 1)]);
    }
}
