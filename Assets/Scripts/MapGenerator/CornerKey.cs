using System;
using UnityEngine;

public readonly struct CornerKey : IEquatable<CornerKey>
{
    private readonly Vector3Int rounded;
    private const float Scale = 1000f;

    public CornerKey(Vector3 worldPos)
    {
        rounded = new Vector3Int(
            Mathf.RoundToInt(worldPos.x * Scale),
            Mathf.RoundToInt(worldPos.y * Scale),
            Mathf.RoundToInt(worldPos.z * Scale)
        );
    }

    public bool Equals(CornerKey other) => rounded.Equals(other.rounded);
    public override bool Equals(object obj) => obj is CornerKey other && Equals(other);
    public override int GetHashCode() => rounded.GetHashCode();
    public static bool operator ==(CornerKey a, CornerKey b) => a.Equals(b);
    public static bool operator !=(CornerKey a, CornerKey b) => !a.Equals(b);
}