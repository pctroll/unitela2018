using UnityEngine;
using System.Collections.Generic;

public enum BSPNodeType
{
    Branch, Leaf
}

public class BSPNode
{
    // public BSPNodeType type;
    public SplitType splitDirection;

    public Rect area;
    public Rect block;
    public Dungeon dungeon;
    public BSPNode left;
    public BSPNode right;

    public BSPNode(Rect area, Dungeon dungeon)
    {
        this.area = area;
        this.dungeon = dungeon;
        // this.type = BSPNodeType.Branch;
        this.block = Rect.zero;
        this.left = null;
        this.right = null;
    }

    public Rect GetBlock()
    {
        if (left == null && right == null)
            return block;
        
        int randNum = Random.Range(0, 99);
        if (randNum % 2 == 0)
            return right.GetBlock();
        return left.GetBlock();

    }

    public static Blob SplitArea(Rect area, int minCutValue)
    {
        int max = (int)Mathf.Max(area.width, area.height);
        if (max < minCutValue)
            return null;
        
        Blob b = new Blob();
        b.splitType = SplitType.Horizontal;
        if (area.width == area.height)
        {
            int randNum = Random.Range(0, 99);
            if (randNum % 2 == 0)
                b.splitType = SplitType.Vertical;
        }
        else if (area.width > area.height)
            b.splitType = SplitType.Vertical;

        float divider, cut;
        divider = Random.Range(0.3f, 0.7f);
        // divider = 0.5f;
        if (b.splitType == SplitType.Horizontal)
        {
            cut = Mathf.RoundToInt(area.height * divider);
            b.areaA.xMin = area.xMin;
            b.areaA.yMin = area.yMin;
            b.areaA.xMax = area.xMax;
            b.areaA.yMax = cut;

            b.areaB.xMin = area.xMin;
            b.areaB.yMin = cut;
            b.areaB.xMax = area.xMax;
            b.areaB.yMax = area.yMax;
        }
        else
        {
            cut = Mathf.RoundToInt(area.width * divider);
            b.areaA.xMin = area.xMin;
            b.areaA.yMin = area.yMin;
            b.areaA.yMax = area.yMax;
            b.areaA.xMax = cut;

            b.areaB.xMin = cut;
            b.areaB.yMin = area.yMin;
            b.areaB.xMax = area.xMax;
            b.areaB.yMax = area.yMax;
        }

        return b;
    }

    public static Rect CreateBlock(Rect area)
    {
        Rect block = new Rect
        {
            xMin = area.xMin + 1f,
            yMin = area.yMin + 1f,
            xMax = area.xMax - 1f,
            yMax = area.yMax - 1f
        };
        return block;
    }

}

public enum SplitType
{
    Horizontal, Vertical
}

[System.Serializable]
public class Blob
{
    public SplitType splitType;
    public Rect areaA;
    public Rect areaB;
}
