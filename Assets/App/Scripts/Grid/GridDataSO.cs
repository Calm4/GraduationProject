using System.Collections.Generic;
using System.IO;
using App.Scripts.Buildings.BuildingsConfigs;
using UnityEditor;
using UnityEngine;

namespace App.Scripts.Grid
{
    [CreateAssetMenu(fileName = "GridData", menuName = "Grid/Grid Data", order = 1)]
    public class GridDataSO : ScriptableObject
    {
        [HideInInspector]
        public List<GridObjectData> gridObjects = new();

        [HideInInspector] public Vector2Int gridSize;

        public void ExportToJson()
        {
            string path = EditorUtility.SaveFilePanel("Save Grid Data as JSON", "", "level_setup", "json");

            if (string.IsNullOrEmpty(path))
            {
                Debug.LogWarning("No file path selected.");
                return;
            }
            
            var dataToSave = new GridSaveData
            {
                gridSize = this.gridSize,
                gridObjects = ConvertPositions(this.gridObjects).ToArray() 
            };

            string json = JsonUtility.ToJson(dataToSave, true);
            File.WriteAllText(path, json);

            Debug.Log($"Grid data exported to JSON at path: {path}");
        }

        private List<GridObjectData> ConvertPositions(List<GridObjectData> objects)
        {
            var convertedObjects = new List<GridObjectData>();

            foreach (var obj in objects)
            {
                var convertedPosition = new Vector3Int(obj.position.x, obj.position.z, obj.position.y);
                var convertedObject = new GridObjectData(obj.buildingConfig, convertedPosition);
                convertedObjects.Add(convertedObject);
            }

            return convertedObjects;
        }

        public void ClearGrid()
        {
            gridObjects.Clear();
        }

        [System.Serializable]
        private class GridSaveData
        {
            public Vector2Int gridSize;
            public GridObjectData[] gridObjects;
        }
    }
}

