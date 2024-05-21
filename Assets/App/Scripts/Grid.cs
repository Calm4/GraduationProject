using System.Collections;
using System.Collections.Generic;
using CodeMonkey.Utils;
using UnityEngine;

public class Grid
{
    private int _width;
    private int _height;
    private float _cellSize;
    private int[,] _gridArray;

    public Grid(int width, int height, float cellSize)
    {
        _width = width;
        _height = height;
        _cellSize = cellSize;

        _gridArray = new int[_width, _height];

        GenerateGrid();
    }

    private void GenerateGrid()
    {
        for (int x = 0; x < _gridArray.GetLength(0); x++)
        {
            for (int z = 0; z < _gridArray.GetLength(1); z++)
            {
                var text = UtilsClass.CreateWorldText(_gridArray[x, z].ToString(), null, GetWorldPosition(x, 0, z) + new Vector3(_cellSize,0,_cellSize) * 0.5f, 30,
                    Color.white, TextAnchor.MiddleCenter);
                text.transform.rotation = Quaternion.Euler(90f, 0f, 0f);

                Debug.DrawLine(GetWorldPosition(x, 0, z), GetWorldPosition(x, 0, z + 1), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(x, 0, z), GetWorldPosition(x + 1, 0, z), Color.white, 100f);
            }
        }
        Debug.DrawLine(GetWorldPosition(0, 0, _height), GetWorldPosition(_width, 0, _height), Color.white, 100f);
        Debug.DrawLine(GetWorldPosition(_width, 0, 0), GetWorldPosition(_width, 0, _height), Color.white, 100f);

    }

    private Vector3 GetWorldPosition(int x, int y, int z)
    {
        return new Vector3(x, y, z) * _cellSize;
    }
}