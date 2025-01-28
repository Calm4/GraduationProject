using App.Scripts.Factories;
using UnityEngine;
using Zenject;

namespace App.Scripts.Enemies
{
    public class EnemyFactory : IEnemyFactory
    {
        private DiContainer _container;

        public EnemyFactory(DiContainer container)
        {
            _container = container;
        }

        public Enemy Create(Enemy prefab, Transform parent)
        {
            return _container.InstantiatePrefabForComponent<Enemy>(prefab, parent);
        }
    }
}