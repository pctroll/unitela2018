﻿using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 
/// </summary>
public class DungeonVisualizer : MonoBehaviour
{
    [SerializeField]
    private DungeonCellStore _cellStore;
    [SerializeField]
    // private Dungeon _dungeon;
    private List<GameObject> cellList;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="grid"></param>
    public void Render(int[,] grid)
    {
        Clear();
        Vector3 cellSize = _cellStore.prefab.GetComponent<SpriteRenderer>().bounds.size;
        GameObject obj;
        SpriteRenderer renderer;
        int gridWidth = grid.GetLength(1);
        int gridHeight = grid.GetLength(0);
        int i, j, counter = 0;
        for (i = 0; i < gridHeight; i++)
        {
            for (j = 0; j < gridWidth; j++)
            {
                obj = Instantiate(_cellStore.prefab, transform);
                renderer = obj.GetComponent<SpriteRenderer>();
                int index = grid[i, j];
                renderer.sprite = _cellStore.spriteList[index];
                Vector3 position = Vector3.zero;
                position.x = (j * cellSize.x);
                position.y = (i * -cellSize.y);
                obj.transform.position = position;
                obj.name = "Cell_" + counter;
                cellList.Add(obj);
                counter++;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void Clear()
    {
        if (cellList == null || cellList.Count == 0)
            return;
        foreach (GameObject obj in cellList)
        {
            obj.transform.parent = null;
            DestroyImmediate(obj);
        }
        cellList.Clear();
    }

    // Use this for initialization
    void Start()
    {
        cellList = new List<GameObject>();
    }
}
