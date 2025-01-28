using UnityEngine;

namespace App.Scripts.Buildings
{
    public interface IBuildingFactory
    {
        Building Create(Building prefab, Transform parent);
    }
}