using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    [SerializeField] private List<GameObject> _placedGameObject = new();

    public int PlaceObject(GameObject prefab, Vector3 position)
    {
        GameObject newObject = Instantiate(prefab);
        newObject.transform.position = position;
        _placedGameObject.Add(newObject);
        return _placedGameObject.Count - 1;
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
