﻿using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using App.Scripts.JsonClasses.Data;

namespace App.Scripts.Grid
{
    [CreateAssetMenu(fileName = "GridData", menuName = "Configs/Grid Data", order = 1)]
    public class GridDataSO : ScriptableObject
    {
        [HideInInspector]
        public List<GridObjectData> gridObjects = new();

       [HideInInspector] public Vector2Int GridSize;

       public void ExportToJson()
       {
#if UNITY_EDITOR
           string path = EditorUtility.SaveFilePanel("Save Grid Data as JSON", "", "level_setup", "json");

           if (string.IsNullOrEmpty(path))
           {
               Debug.LogWarning("No file path selected.");
               return;
           }

           var dataToSave = new GridSaveDataJson
           {
               gridSize = GridSize,
               gridObjects = ConvertToSerializable(gridObjects)
           };

           string json = JsonUtility.ToJson(dataToSave, true);
           File.WriteAllText(path, json);

           Debug.Log($"Grid data exported to JSON at path: {path}");
#else
    Debug.LogError("ExportToJson is only supported in the Unity Editor.");
#endif
       }


        private List<GridObjectSerializableData> ConvertToSerializable(List<GridObjectData> objects)
        {
            var convertedObjects = new List<GridObjectSerializableData>();

            foreach (var obj in objects)
            {
                var convertedPosition = new Vector3Int(obj.Position.x, obj.Position.z, obj.Position.y);
                var serializableObject = new GridObjectSerializableData(obj.Building.BuildingConfig.ID, convertedPosition);
                convertedObjects.Add(serializableObject);
            }

            return convertedObjects;
        }

        public void ClearGrid()
        {
            gridObjects.Clear();
        }
    }
}
