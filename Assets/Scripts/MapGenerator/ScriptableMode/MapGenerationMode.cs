using UnityEngine;

public abstract class MapGenerationMode : ScriptableObject
{
    public string modeName;
    public abstract void Generate(MapGenerator generator);
}
