using System.IO;
using App.Scripts.Buildings;
using UnityEngine;

namespace App.Scripts.Placement.LevelCreatingWindow
{
    public class GridInitializer : MonoBehaviour
    {
        [SerializeField] private GridData gridData;
        [SerializeField] private Building mountainPrefab;
        [SerializeField] private Building forestPrefab;

        private void Start()
        {
            InitializeGrid();
        }

        private void InitializeGrid()
        {
            string path = Path.Combine(Application.dataPath, "App/Resources/LevelConfigs", "level_setup_.json");
            if (!File.Exists(path))
            {
                Debug.LogError("Configuration file not found!");
                return;
            }

            string json = File.ReadAllText(path);
            var objects = JsonUtility.FromJson<Wrapper<GridObjectData>>(json);

            foreach (var obj in objects.items)
            {
                Building prefab = true ? forestPrefab : mountainPrefab;
                Vector2 objectSize = new Vector2(obj.buildingConfig.size.x, obj.buildingConfig.size.y);
                Vector3Int position = obj.position;

                gridData.AddObjectAt(position, objectSize, prefab);
                PlaceVisualObject(position, prefab, obj.buildingConfig.size);
            }
        }

        private void PlaceVisualObject(Vector3Int position, Building prefab, Vector2Int size)
        {
            Building buildingInstance = Instantiate(prefab, new Vector3(position.x, 0, position.z), Quaternion.identity);
            /*buildingInstance.SetSize(new Vector2(size.x, size.y));*/
        }

        [System.Serializable]
        private class Wrapper<T>
        {
            public T[] items;
        }
    }
}