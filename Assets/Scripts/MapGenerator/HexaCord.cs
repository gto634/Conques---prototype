using System;
using UnityEngine;

public class HexaCord
{
    public int x;
    public int y;
    public int z;

    public HexaCord(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public static HexaCord operator +(HexaCord a, HexaCord b)
    {
        return new HexaCord(a.x + b.x, a.y + b.y, a.z + b.z);
    }

    public override bool Equals(object obj)
    {
        if (obj is not HexaCord other) return false;
        return x == other.x && y == other.y && z == other.z;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(x, y, z);
    }

    public static readonly HexaCord[] directions =
    {
        new HexaCord(1, -1, 0),
        new HexaCord(1, 0, -1),
        new HexaCord(0, 1, -1),
        new HexaCord(-1, 1, 0),
        new HexaCord(-1, 0, 1),
        new HexaCord(0, -1, 1)
    };
}
