using UnityEngine;
using System.Collections.Generic;

public class GenerateMap : MonoBehaviour
{
    public GameObject map;
    public List<GameObject> tilePrefab;

    [Header("Map size")]
    public int XSize = 15;
    public int YSize = 15;

    public float offsetX = 0.5f;
    public float offsetZ = 0.5f;    

    void Start()
    {
        float realOffsetZ = 0;

        for (int i = 0; i < XSize; i++)
        {
            if(i % 2 == 0)
            {
                realOffsetZ = offsetZ;
            }
            else
            {
                realOffsetZ = 0f;
            }

            for (int j = 0; j < YSize; j++)
            {
                int randomIndex = Random.Range(0, tilePrefab.Count);
                GameObject tile = Instantiate(tilePrefab[randomIndex], new Vector3(i * offsetX, 0, j + realOffsetZ), Quaternion.identity);
                tile.transform.parent = map.transform;
                tile.transform.localRotation = Quaternion.Euler(0, 90, 0);
            }
        }
    }
}
