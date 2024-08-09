using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace App.Scripts.Grid
{
    [CreateAssetMenu(fileName = "GridData", menuName = "Grid/Grid Data", order = 1)]
    public class GridDataSO : ScriptableObject
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

            // Создаем экземпляр класса-обертки и заполняем его данными
            var dataToSave = new GridSaveData
            {
                gridSize = this.gridSize,
                gridObjects = this.gridObjects.ToArray()
            };

            // Сериализуем данные в JSON
            string json = JsonUtility.ToJson(dataToSave, true);
            File.WriteAllText(path, json);

            Debug.Log($"Grid data exported to JSON at path: {path}");
        }

        public void ClearGrid()
        {
            gridObjects.Clear();
        }

        // Класс-обертка для сохранения данных
        [System.Serializable]
        private class GridSaveData
        {
            public Vector2Int gridSize;
            public GridObjectData[] gridObjects;
        }
    }
}


