using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Map Generation/Catan")]
public class CatanGenerationMode : MapGenerationMode
{
    public int layers = 3;
    public override void Generate(MapGenerator generator)
    {
        if (generator.tiles.Count == 0)
            return;

        generator.ResetMap();

        List<int> tilePool = new List<int>();

        for (int i = 0; i < layers; i++)
        {
            List<HexaCord> freeCords = generator.nodeGrid.GetFreeTileCords().ToList<HexaCord>();

            for (int j = 0; j < freeCords.Count; j++)
            {

                if (tilePool.Count == 0)
                {
                    tilePool = generator.GetTilePool(1f);

                    if (tilePool.Count == 0)
                        return;
                }

                HexaCord cord = freeCords[j];
                int prefabIndex = tilePool[tilePool.Count - 1];
   
                tilePool.RemoveAt(tilePool.Count - 1);
                generator.InstanciateTile(cord, generator.tiles[prefabIndex].prefab);
            }
        }

        generator.InstanciateWaterBorder();
    }
}