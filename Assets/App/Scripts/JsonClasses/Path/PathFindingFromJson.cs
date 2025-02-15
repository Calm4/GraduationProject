using System.Collections.Generic;
using System.Linq;
using App.Scripts.Buildings;
using App.Scripts.Grid;
using App.Scripts.JsonClasses.Data;
using App.Scripts.TurnsBasedSystem;
using UnityEngine;

namespace App.Scripts.JsonClasses.Path
{
    public class PathFindingFromJson
    {
        private readonly GridManager _gridManager;
        private readonly JsonFilesDataBase _jsonFilesDataBase;
        private readonly Building _spawner;
        private readonly Building _castle;
        private readonly Building _pathway;


        public PathFindingFromJson(GridManager gridManager, JsonFilesDataBase jsonFilesDataBase, Building spawner,
            Building castle, Building pathway)
        {
            _gridManager = gridManager;
            _jsonFilesDataBase = jsonFilesDataBase;
            _spawner = spawner;
            _castle = castle;
            _pathway = pathway;
        }

        public List<Vector2> FindPathFromJson()
        {
            GridObjectContainerJson gridObjectContainerJson =
                JsonUtility.FromJson<GridObjectContainerJson>(_jsonFilesDataBase.BuildingsJsonFile.text);

            var gridObjects = GetGridObjectsFromJson(gridObjectContainerJson);

            Vector2 spawnerPosition = PathFinding.FindObjectPosition(gridObjects, _spawner);
            Vector2 castlePosition = PathFinding.FindObjectPosition(gridObjects, _castle);

            if (spawnerPosition == new Vector2Int(-1, -1) || castlePosition == new Vector2Int(-1, -1))
            {
                Debug.LogError("Не удалось найти спавнер или замок в JSON.");
                return null;
            }

            List<Vector2> path =
                PathFinding.GeneratePath(_gridManager, gridObjects, _pathway, _castle, spawnerPosition,
                    castlePosition);

            var s = path.Aggregate("", (current, p) => current + ("[" + p.x + ":" + p.y + "]\n"));
            //Debug.Log(s);

            //Debug.Log("Optimized Path length: " + path.Count);


            return path;
        }
        
        private static Dictionary<Vector2, int> GetGridObjectsFromJson(GridObjectContainerJson gridObjectContainerJson)
        {
            Dictionary<Vector2, int> gridObjects = new Dictionary<Vector2, int>();

            foreach (var gridObject in gridObjectContainerJson.gridObjects)
            {
                Vector2 position = new Vector2(gridObject.position.x, gridObject.position.z);
                gridObjects[position] = gridObject.buildingConfigID;
            }

            return gridObjects;
        }
    }
}