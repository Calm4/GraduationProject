using System.Collections.Generic;
using UnityEngine;

namespace App.Scripts.Placement
{
    public class ObjectPlacer : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _placedGameObject = new();

        public (GameObject placedObject, int index) PlaceObject(GameObject prefab, Vector3 position)
        {
            GameObject newObject = Instantiate(prefab);
            newObject.transform.position = position;
            _placedGameObject.Add(newObject);
            int index = _placedGameObject.Count - 1;
            return (newObject, index);
        }

        public void RemoveObjectAt(int gameObjectIndex)
        {
            if (_placedGameObject.Count <= gameObjectIndex || !_placedGameObject[gameObjectIndex])
            {
                return;
            }
            Destroy(_placedGameObject[gameObjectIndex]);
            _placedGameObject[gameObjectIndex] = null;
        }
    }
}
