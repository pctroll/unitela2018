using UnityEngine;
using System.Collections.Generic;

public class Dungeon : MonoBehaviour
{
    [Range(3, 200)]
    public int maxHeight;
    [Range(3, 200)]
    public int maxWidth;
    /// <summary>
    /// Minimum Acceptable Size
    /// </summary>
    /// [
    [Range(2, 100)]
    public int minAcceptSize;
    /// <summary>
    /// Dungeon area
    /// </summary>
    private Rect area;
    // public Dictionary<int, List<BSPNode>> tree;
    // public HashSet<BSPNode> leaves;

    public delegate Blob Split(Rect area);
    public Split splitCall;

    public BSPNode root;

    public int[,] grid;

    private Queue<BSPNode> _queue;
    private Stack<BSPNode> _stack;

    public void Init()
    {
        BSPNode.cutVal = minAcceptSize;
        // print("Dungeon.Init");
        area = new Rect();
        area.xMin = 0;
        area.yMin = 0;
        area.xMax = maxWidth;
        area.yMax = maxHeight;
        grid = new int[maxHeight, maxWidth];
        root = new BSPNode(area, this);
        _queue = new Queue<BSPNode>();
        _stack = new Stack<BSPNode>();
        _queue.Enqueue(root);
        // _stack.Push(root);
    }

    public void BindBlocks(Rect a, Rect b, SplitType split)
    {
        //print("BindBlocks");
        int originX, originY, targetX, targetY;
        if (split == SplitType.Horizontal)
        {
            originX = (int)Random.Range(a.xMin + 1, a.xMax - 1);
            originY = (int)a.center.x;
            targetX = originX;
            targetY = (int)b.center.y;
        }
        else
        {
            originX = (int)a.center.x;
            originY = (int)Random.Range(a.yMin + 1, a.yMax - 1);
            targetX = (int)b.center.x;
            targetY = originY;
        }
        //print("origin: (" + originX + "," + originY + ")");
        //print("target: (" + targetX + "," + targetY + ")");

        int i, j;
        for (i = originY; i <= targetY; i++)
        {
            // print("i: " + i);
            for (j = originX; j <= targetX; j++)
            {
                // print("j: " + j);
                // print("(" + j + "," + i + ")");
                if (grid[i,j] == 0)
                    grid[i, j] = 1;
            }
        }

    }

    public void BlockToGrid(BSPNode node)
    {
        int i, j, x, y, w, h;
        x = (int)node.block.xMin;
        y = (int)node.block.yMin;
        w = (int)node.block.xMax;
        h = (int)node.block.yMax;
        
        for (i = y; i < h; i++)
        {
            for (j = x; j < w; j++)
            {
                grid[i, j] = 1;
            }
        }
    }


    public void Build()
    {
        Init();
        while (_queue.Count != 0)
        {
            BSPNode n = _queue.Dequeue();
            
            Blob b = BSPNode.SplitArea(n.area, minAcceptSize);
            if (b == null)
            {
                n.block = BSPNode.CreateBlock(n.area);
                BlockToGrid(n);
                continue;
            }
            
            n.splitDirection = b.splitType;
            n.left = new BSPNode(b.areaA, this);
            n.right = new BSPNode(b.areaB, this);

            _queue.Enqueue(n.left);
            _queue.Enqueue(n.right);

            // _stack.Push(n.left);
            // _stack.Push(n.right);
        }

        
    }

    public void BindNodes()
    {
    }

    private void Start()
    {
        grid = new int[1, 1];        
    }

}
