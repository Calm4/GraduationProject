using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace App.Scripts.Placement.LevelCreatingWindow
{
    [CreateAssetMenu(fileName = "GridData", menuName = "Grid/Grid Data", order = 1)]
    public class GridDataAsset : ScriptableObject
    {
        [HideInInspector]
        public List<GridObjectData> gridObjects = new List<GridObjectData>();

        [HideInInspector] public Vector2Int gridSize;

        public void ExportToJson()
        {
            string path = EditorUtility.SaveFilePanel("Save Grid Data as JSON", "", "level_setup", "json");

            if (string.IsNullOrEmpty(path))
            {
                Debug.LogWarning("No file path selected.");
                return;
            }

            string json = JsonUtility.ToJson(new Wrapper<GridObjectData> { items = gridObjects.ToArray() }, true);

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
}

