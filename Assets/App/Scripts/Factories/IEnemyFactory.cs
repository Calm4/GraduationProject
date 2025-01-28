using App.Scripts.Enemies;
using UnityEngine;

namespace App.Scripts.Factories
{
    public interface IEnemyFactory
    {
        Enemy Create(Enemy prefab, Transform parent);
    }
}