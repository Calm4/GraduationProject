using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "GridData", menuName = "Grid/Grid Data", order = 1)]
public class GridDataAsset : ScriptableObject
{
    [TableList]
    public List<GridObjectData> gridObjects = new List<GridObjectData>();

    [Button("Export to JSON")]
    public void ExportToJson()
    {
        string json = JsonUtility.ToJson(new Wrapper<GridObjectData> { items = gridObjects.ToArray() }, true);
        string path = Path.Combine(Application.dataPath, "Assets/App/Resources/LevelConfigs", "level_setup_.json");
        File.WriteAllText(path, json);
        Debug.Log($"Grid data exported to JSON at path: {path}");
    }

    [Button("Clear Grid")]
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