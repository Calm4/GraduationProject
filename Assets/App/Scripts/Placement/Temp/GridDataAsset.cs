using System.Collections.Generic;
using System.IO;
using App.Scripts.Buildings.BuildingsConfigs;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEditor;

[CreateAssetMenu(fileName = "GridData", menuName = "Grid/Grid Data", order = 1)]
public class GridDataAsset : ScriptableObject
{
    [TableList]
    public List<GridObjectData> gridObjects = new List<GridObjectData>();

    [HideInInspector] public Vector2Int gridSize = new Vector2Int(10, 10);

    public void ExportToJson()
    {
        // Open file save panel to select the save location
        string path = EditorUtility.SaveFilePanel("Save Grid Data as JSON", "", "level_setup", "json");

        // Check if a valid path is selected
        if (string.IsNullOrEmpty(path))
        {
            Debug.LogWarning("No file path selected.");
            return;
        }

        // Convert data to JSON
        string json = JsonUtility.ToJson(new Wrapper<GridObjectData> { items = gridObjects.ToArray() }, true);

        // Write JSON data to the selected file
        File.WriteAllText(path, json);
        Debug.Log($"Grid data exported to JSON at path: {path}");
    }

    public void ClearGrid()
    {
        gridObjects.Clear();
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] items;
    }
}

