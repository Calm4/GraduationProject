using System.Collections.Generic;
using System.IO;
using App.Scripts.Buildings.BuildingsConfigs;
using UnityEditor;
using UnityEngine;

namespace App.Scripts.Grid
{
    [CreateAssetMenu(fileName = "GridData", menuName = "Configs/Grid Data", order = 1)]
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
                gridObjects = ConvertToSerializable(gridObjects).ToArray() 
            };

            string json = JsonUtility.ToJson(dataToSave, true);
            File.WriteAllText(path, json);

            Debug.Log($"Grid data exported to JSON at path: {path}");
        }

        private List<GridObjectSerializableData> ConvertToSerializable(List<GridObjectData> objects)
        {
            var convertedObjects = new List<GridObjectSerializableData>();

            foreach (var obj in objects)
            {
                var convertedPosition = new Vector3Int(obj.position.x, obj.position.z, obj.position.y);
                var serializableObject = new GridObjectSerializableData(obj.buildingConfig.ID, convertedPosition);
                convertedObjects.Add(serializableObject);
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
            public GridObjectSerializableData[] gridObjects;
        }
    }
}
