using App.Scripts.Buildings;
using App.Scripts.Enemies;
using App.Scripts.Modifiers.Data;
using UnityEngine;
using System.Collections.Generic;

namespace App.Scripts.Modifiers.Strategies
{
    public class RangeUpdateStrategy : AbstractModifierUpdateStrategy
    {
        private List<Enemy> enemyQueue = new List<Enemy>();

        public override void UpdateModifier(BaseModifierData data)
        {
            if (data is RangeModifierData rangeData)
            {
                if (OwnerBuilding == null)
                {
                    Debug.LogError("RangeUpdateStrategy: OwnerBuilding не установлен!");
                    return;
                }

                float radius = rangeData.currentRange;
                Vector3 center = OwnerBuilding.transform.position;

                Collider[] hits = Physics.OverlapSphere(center, radius);
                List<Enemy> currentEnemies = new List<Enemy>();

                foreach (Collider hit in hits)
                {
                    Enemy enemy = hit.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        currentEnemies.Add(enemy);
                    }
                }

                enemyQueue.RemoveAll(e => e == null || !currentEnemies.Contains(e));

                foreach (Enemy enemy in currentEnemies)
                {
                    if (!enemyQueue.Contains(enemy))
                    {
                        enemyQueue.Add(enemy);
                    }
                }

                enemyQueue.Sort((e1, e2) => e2.TraveledDistance.CompareTo(e1.TraveledDistance));
            }
            else
            {
                Debug.LogError("RangeUpdateStrategy: Неверный тип данных. Ожидался RangeModifierData.");
            }
        }
        
        public Enemy GetTargetEnemy()
        {
            return enemyQueue.Count > 0 ? enemyQueue[0] : null;
        }

        public List<Enemy> GetEnemiesInRange()
        {
            return enemyQueue;
        }
    }
}
