using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject map;
    public NodeGrid nodeGrid;

    public MapGenerationMode generationMode;

    public List<TileEntry> tiles;

    private float tileOffset;

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
        tileOffset = Mathf.Max(size.x,size.z) * 0.5f;
       }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            generationMode.Generate(this);
        } 
    }

    public void InstanciateTile(HexaCord cord, int prefabIndex)
    {
        MapNode node = nodeGrid.CreateNewNode(cord);
        Vector3 position = HexaToWorld(node.hexaCord);

        GameObject firstTile = Instantiate(tiles[prefabIndex].prefab, position, Quaternion.identity, map.transform);
        ApplyTileRotation(firstTile);
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

    public Vector3 HexaToWorld(HexaCord hex)
    {
        float size = tileOffset;

        float worldX = size * (1.5f * hex.x);
        float worldZ = size * (Mathf.Sqrt(3f) * (hex.z + hex.x * 0.5f));

        return new Vector3(worldX, transform.position.y, worldZ);
    }

    public void ApplyTileRotation(GameObject tile)
    {
        tile.transform.Rotate(tileRotations[Random.Range(0, tileRotations.Length - 1)]);
    }
}
