using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;
using static UnityEngine.RectTransform;

public class MapGenerator : MonoBehaviour
{
    public GameObject map;
    public NodeGrid nodeGrid;

    public MapGenerationMode generationMode;

    public List<TileEntry> tiles;
    private int waterIndex = -1;

    public List<PortEntry> ports;

    public List<MapNumbersEntry> mapNumbers;
    public Dictionary<int, List<int>> bucketsNumbers = new Dictionary<int, List<int>>();

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
        MapGenerator.tileOffset = Mathf.Max(size.x, size.z) * 0.5f;
        MapGenerator.worldY = transform.position.y;
        MapGenerator.tileMargin = 0.1f; // meh

        for (int i = 0; i < tiles.Count; i++)
        {
            TileEntry tile = tiles[i];
            if(tile.isWater)
            {
                waterIndex = i;
                break;
            }
        }
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

        foreach (TileNode node in nodeGrid.tileGrid.Values)
        {
            for (int i = 0; i < node.corners.Count; i++)
            {
                CornerNode corner = node.corners[i];
                corner.DrawGizmo();
            }

            for (int i = 0; i < node.edges.Count; i++)
            {
                EdgeNode edge = node.edges[i];
                edge.DrawGizmo();
            }
        }
    }

    public void InstanciateTile(HexaCord cord, TileEntry tileEntry)
    {
        TileNode node = nodeGrid.CreateNewNode(cord, tileEntry.isWater, tileEntry.hasNumber);
        if (tileEntry.hasNumber)
        {
            AssignNumbersToTile(node);
        }

        Vector3 position = node.worldPosition;

        GameObject tile = Instantiate(tileEntry.prefab, position, Quaternion.identity, map.transform);
        TileBehavior behavior = tile.GetComponent<TileBehavior>();
        behavior.node = node;
        ApplyTileRotation(tile);
    }

    public void InstanciateWaterBorder()
    {
        if (waterIndex == -1)
            return;

        List<HexaCord> freeCords = nodeGrid.GetFreeTileCords().ToList<HexaCord>();
        for (int i = 0; i < freeCords.Count; i++)
        {
            HexaCord cord = freeCords[i];
            InstanciateTile(cord, tiles[waterIndex]);
        }
    }

    public void AssignNumbersToTile(TileNode tile) // something may be wrong here
    {
        if (bucketsNumbers.Count == 0)
            RefreshBucketNumber();

        int maxBucket = 6;
        int maxCornerOddsTarget = 12;
        int maxOddToAvoidRepetition = 5;
        for (int i = 0; i < tile.corners.Count; i++)
        {
            CornerNode corner = tile.corners[i];
            int cornerOdds = corner.GetTilesOdds();
            int cornerMaxOdd = corner.GetTileMaxOdd();
            maxBucket = Mathf.Min(maxBucket, maxCornerOddsTarget - cornerOdds);
            if (maxOddToAvoidRepetition <= cornerMaxOdd)
            {
                Mathf.Min(maxOddToAvoidRepetition - 1, maxBucket);
            }
        }
        maxBucket = Mathf.Max(1, maxBucket);

        int selectedBucket = -1;
        int startBucket = Random.Range(1, maxBucket + 1);

        for (int i = 0; i < maxBucket; i++)
        {
            int bucketIndex = (startBucket + i - 1) % maxBucket + 1;
            if (bucketsNumbers.TryGetValue(bucketIndex, out List<int> numbers) && numbers.Count > 0)
            {
                selectedBucket = bucketIndex;
                break;
            }
        }

        if (selectedBucket == -1)
        {
            var existingKeys = bucketsNumbers.Keys.OrderBy(k => k).ToList();
            if (existingKeys.Count > 0)
            {
                int startIndex = Random.Range(0, existingKeys.Count);
                for (int i = 0; i < existingKeys.Count; i++)
                {
                    int key = existingKeys[(startIndex + i) % existingKeys.Count];
                    if (bucketsNumbers[key].Count > 0)
                    {
                        selectedBucket = key;
                        break;
                    }
                }
            }
        }

        var selectedList = bucketsNumbers[selectedBucket];
        tile.tileValue = selectedList[selectedList.Count - 1];
        selectedList.RemoveAt(selectedList.Count - 1);
        if (selectedList.Count == 0)
            bucketsNumbers.Remove(selectedBucket);
    }

    public void DispatchPorts(float sizeScaling)
    {
        List<int> portPool = GetPortPool(sizeScaling);

        List<TileNode> nodes = nodeGrid.tileGrid.Values.ToList<TileNode>();
        for (int i = 0; i < portPool.Count; i++)
        {
            int portIndex = portPool[i];

            int preSelectionSize = nodes.Count;
            for (int j = 0; j < preSelectionSize; j++)
            {
                int index = Random.Range(0, nodes.Count - 1);
                TileNode tile = nodes[index];
                nodes.RemoveAt(index);
                if (CanDispatchPort(tile, out EdgeNode edge))
                {
                    InstanciatePortEdge(edge,tile,ports[portIndex].prefab);
                    break;
                }
            }
        }
    }

    public bool CanDispatchPort(TileNode tile, out EdgeNode edge)
    {
        edge = null;

        if (tile.isWater)
            return false;

        List<EdgeNode> validEdges = new List<EdgeNode>();
        foreach (EdgeNode testEdge in tile.edges)
        {
            if (testEdge.IsCoastal() && testEdge.HasNoCornerPorts() && testEdge.HasNoCornerNearPorts())
            {
                validEdges.Add(testEdge);
            }
        }

        if (validEdges.Count == 0)
            return false;

        int randomIndex = Random.Range(0, validEdges.Count);
        edge = validEdges[randomIndex];
        return true;
    }

    public void InstanciatePortEdge(EdgeNode edge, TileNode tile, GameObject prefab)
    {
        edge.cornerA.hasPort = true;
        edge.cornerB.hasPort = true;
        GameObject port1 = Instantiate(prefab, edge.cornerA.worldPosition, Quaternion.identity, map.transform);
        GameObject port2 = Instantiate(prefab, edge.cornerB.worldPosition, Quaternion.identity, map.transform);
        // rotate it to look in the opposite direction of the arg tile ?
    }

    public void ResetMap()
    {
        Destroy(map);
        map = new GameObject("Map");
        bucketsNumbers.Clear();
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

    public List<int> GetPortPool(float mult)
    {
        List<int> portPool = new List<int>();

        for (int prefabIndex = 0; prefabIndex < ports.Count; prefabIndex++)
        {
            int poolAmount = (int)Mathf.Ceil(ports[prefabIndex].amount * mult); // why does ceil returns float...

            for (int i = 0; i < poolAmount; i++)
            {
                portPool.Add(prefabIndex);
            }
        }

        List<int> shuffledPortPool = portPool.OrderBy(x => Random.value).ToList();

        return shuffledPortPool;
    }

    public void RefreshBucketNumber()
    {
        for (int index = 0; index < mapNumbers.Count; index++)
        {
            MapNumbersEntry numberEntry = mapNumbers[index];
            int bucket = numberEntry.GetOdd();

            for (int i = 0; i < numberEntry.amount; i++)
            {
                if (bucketsNumbers.TryGetValue(bucket, out List<int> numbers))
                {
                    numbers.Add(numberEntry.value);
                }
                else
                {
                    bucketsNumbers[bucket] = new List<int>();
                    bucketsNumbers[bucket].Add(numberEntry.value);
                }
            }
        }
    }

    public void ApplyTileRotation(GameObject tile)
    {
        tile.transform.Rotate(tileRotations[Random.Range(0, tileRotations.Length - 1)]);
    }
}
