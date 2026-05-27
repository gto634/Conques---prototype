using UnityEngine;

[System.Serializable]
public class TileEntry
{
    public GameObject prefab;
    public bool isWater;
    public bool hasNumber;
    public int amount;

    private static readonly Vector3[] tileRotations =
    {
        new Vector3(0,30,0),
        new Vector3(0,90,0),
        new Vector3(0,150,0),
        new Vector3(0,210,0),
        new Vector3(0,270,0),
        new Vector3(0,330,0)
    };

    static public void ApplyTileRotation(GameObject tile)
    {
        tile.transform.Rotate(tileRotations[Random.Range(0, tileRotations.Length - 1)]);
    }
}