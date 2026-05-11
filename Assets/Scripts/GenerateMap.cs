using UnityEngine;
using System.Collections.Generic;

public class GenerateMap : MonoBehaviour
{
    public GameObject map;
    public List<GameObject> tilePrefab;

    public float offsetX = 0.5f;
    public float offsetZ = 0.5f;    

    void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                int randomIndex = Random.Range(0, tilePrefab.Count);
                GameObject tile = Instantiate(tilePrefab[randomIndex], new Vector3(i + offsetX, 0, j + offsetZ), Quaternion.identity);
                tile.transform.parent = map.transform;
            }
        }
    }
}
