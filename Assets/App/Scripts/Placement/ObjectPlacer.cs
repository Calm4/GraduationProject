using System.Collections.Generic;
using App.Scripts.Buildings;
using UnityEngine;

namespace App.Scripts.Placement
{
    public class ObjectPlacer : MonoBehaviour
    {
        [SerializeField] private List<Building> _placedGameObject = new();

        public Building PlaceObject(Building prefab, Vector3 position, BasicBuildingConfig config)
        {
            Building newObject = Instantiate(prefab);
            newObject.Initialize(config);
            newObject.transform.position = position;
            _placedGameObject.Add(newObject);
            return newObject;
        }

        public void RemoveObject(Building placedObject)
        {
            int index = _placedGameObject.IndexOf(placedObject);
            if (index >= 0 && index < _placedGameObject.Count)
            {
                Destroy(_placedGameObject[index].gameObject);
                _placedGameObject[index] = null;
            }
        }

        public int GetIndexOfPlacedObject(Building placedObject)
        {
            return _placedGameObject.IndexOf(placedObject);
        }
    }
}