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
    [Range(2, 100)]
    public int minAcceptSize;
    [Space(2)]
    [Header("Visual Settings")]
    [SerializeField]
    private int _mapIndex = 0;
    [SerializeField]
    private int _blockIndex = 1;
    [SerializeField]
    private int _bridgeIndex = 2;
    
    /// <summary>
    /// Dungeon area
    /// </summary>
    private Rect area;

    public delegate Blob Split(Rect area);
    public Split splitCall;

    public BSPNode root;

    public int[,] grid;

    private Queue<BSPNode> _queue;
    private Stack<BSPNode> _stack;
    private Stack<int> _tempStack;
    private int _tempCounter;

    public void Init()
    {
        area = new Rect();
        area.xMin = 0;
        area.yMin = 0;
        area.xMax = maxWidth;
        area.yMax = maxHeight;
        grid = new int[maxHeight, maxWidth];
        int i, j;
        for (i = 0; i < maxHeight; i++)
        {
            for (j = 0; j < maxWidth; j++)
            {
                grid[i, j] = _mapIndex;
            }
        }
        root = new BSPNode(area, this);
        _queue = new Queue<BSPNode>();
        _stack = new Stack<BSPNode>();
        _queue.Enqueue(root);
        _stack.Push(root);

        _tempStack = new Stack<int>();
        _tempCounter++;
        _tempStack.Push(_tempCounter);
    }

    public void BindBlocks(Rect a, Rect b, SplitType split)
    {
        int i, j;
        int origin, target;

        if (split == SplitType.Horizontal)
        {
            // connect with VERTICAL line
            j = (int)a.center.x;
            origin = (int)a.center.y;
            target = (int)b.center.y;
            print("o:" + origin + "   t:" + target);
            print("y: " + j);
            for (i = origin; i <= target; i++)
            {
                grid[i, j] = _bridgeIndex;
            }
        }
        else
        {
            // connect with HORIZONTAL line
            i = (int)a.center.y;
            origin = (int)a.center.x;
            target = (int)b.center.x;
            print("o:" + origin + "   t:" + target);
            print("x: " + i);
            for (j = origin; j <= target; j++)
            {
                grid[i, j] = _bridgeIndex;
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
                grid[i, j] = _blockIndex;
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

            _stack.Push(n.left);
            _stack.Push(n.right);
        }

        while (_stack.Count != 0)
        {
            BSPNode n = _stack.Pop();
            if (n.left == null)
                continue;
            
            BindBlocks(n.left.area, n.right.area, n.splitDirection);
        }
    }

    private void Start()
    {
        grid = new int[1, 1];        
    }

}
