using UnityEngine;
using System.Collections.Generic;

public class BoardGenerationState : BaseState
{
    private GameManagerScript manager;

    private GameObject map;
    private List<GameObject> tilePrefab;
    private int XSize;
    private int YSize;
    private float offsetX;
    private float offsetZ;

    public BoardGenerationState(GameManagerScript gameManager, GameObject mapContainer, List<GameObject> prefabs, int xSize, int ySize, float offX, float offZ)
    {
        manager = gameManager;
        map = mapContainer;
        tilePrefab = prefabs;
        XSize = xSize;
        YSize = ySize;
        offsetX = offX;
        offsetZ = offZ;
    }

    public void Enter()
    {
        Debug.Log("START : Génération du plateau");
            float realOffsetZ = 0;

        if (tilePrefab == null || tilePrefab.Count == 0)
        {
            Debug.LogError("Aucun Prefab de tuile");
            return;
        }

        for (int i = 0; i < XSize; i++)
            {
                if (i % 2 == 0)
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
                    GameObject tile = Object.Instantiate(tilePrefab[randomIndex], new Vector3(i * offsetX, 0, j + realOffsetZ), Quaternion.identity);
                    tile.transform.parent = map.transform;
                    tile.transform.localRotation = Quaternion.Euler(0, 90, 0);
                }
            }
    }

    public void Execute()
    {
        if (Time.timeSinceLevelLoad > 2f)
        {
            manager.ChangeState(manager.DiceRollState);
        }
    }

    public void Exit()
    {
        Debug.Log("Plateau généré avec succès !");
    }
}