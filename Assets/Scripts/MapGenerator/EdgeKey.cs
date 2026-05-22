using System;

public readonly struct EdgeKey : IEquatable<EdgeKey>
{
    private readonly (CornerKey, CornerKey) pair;

    public EdgeKey(CornerKey a, CornerKey b)
    {
        if (a.GetHashCode() < b.GetHashCode())
            pair = (a, b);
        else
            pair = (b, a);
    }

    public bool Equals(EdgeKey other) => pair.Equals(other.pair);
    public override bool Equals(object obj) => obj is EdgeKey other && Equals(other);
    public override int GetHashCode() => pair.GetHashCode();
}