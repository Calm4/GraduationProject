using System;
using UnityEngine;

[Serializable]
public class GridObjectData
{
    public string type; // Тип объекта (например, "Mountain" или "Building")
    public Vector3Int position; // Позиция объекта на сетке
    public Vector2Int size; // Размер объекта (например, 2x2)

    public GridObjectData(string type, Vector3Int position, Vector2Int size)
    {
        this.type = type;
        this.position = position;
        this.size = size;
    }
}