using UnityEngine;
using Zenject;

namespace App.Scripts.Buildings
{
    public class BuildingFactory : IBuildingFactory
    {
        private DiContainer _container;

        public BuildingFactory(DiContainer container)
        {
            _container = container;
        }

        public Building Create(Building prefab, Transform parent)
        {
            return _container.InstantiatePrefabForComponent<Building>(prefab, parent);
        }
    }
}