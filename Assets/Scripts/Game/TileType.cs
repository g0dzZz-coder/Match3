using UnityEngine;
using System;

[Serializable]
public class TileType : IComparable
{
    public string name;
    public Sprite sprite;
    public Color color;

    public int CompareTo(object obj)
    {
        if (obj == null)
            return 1;

        TileType otherTile = obj as TileType;

        if (otherTile != null)
        {
            return string.Compare(this.name, otherTile.name, StringComparison.CurrentCulture);
        }
        else
        {
            throw new ArgumentException("[TileType] Object is not a TileType");
        }
    }
}
