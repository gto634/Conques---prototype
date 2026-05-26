using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Map Generation/Random")]
public class RandomGenerationMode : MapGenerationMode
{
    public float mapSizeMultiplicator = 1f;
    public override void Generate(MapGenerator generator)
    {
        if (generator.tiles.Count == 0)
            return;

        generator.ResetMap();

        List<int> tilePool;

        for (float remainingSize = mapSizeMultiplicator; remainingSize > 0; remainingSize--)
        {
            if (remainingSize < 1f)
            {
                tilePool = generator.GetTilePool(remainingSize);
            }
            else
            {
                tilePool = generator.GetTilePool(1f);
            }

            if (tilePool.Count == 0)
                return;

            for (int i = 1; i < tilePool.Count; i++)
            {
                HexaCord cord = generator.nodeGrid.GetRandomFreeTileCord();
                int prefabIndex = tilePool[i];
                TileEntry tile = generator.tiles[prefabIndex];
                generator.InstanciateTile(cord, tile);
            }
        }

        generator.InstanciateWaterBorder();
        generator.DispatchPorts(mapSizeMultiplicator);
    }
}