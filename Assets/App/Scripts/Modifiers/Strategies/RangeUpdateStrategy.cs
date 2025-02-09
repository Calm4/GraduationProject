using App.Scripts.Buildings;
using App.Scripts.Modifiers.Data;
using UnityEngine;

namespace App.Scripts.Modifiers.Strategies
{
    public class RangeUpdateStrategy : AbstractModifierUpdateStrategy
    {

        public override  void UpdateModifier(BaseModifierData data)
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
                
                // Используем Physics.OverlapSphere для поиска коллайдеров в радиусе
                Collider[] hits = Physics.OverlapSphere(center, radius);
                foreach (Collider hit in hits)
                {
                    EnemyClick enemy = hit.GetComponent<EnemyClick>();
                    if (enemy != null)
                    {
                        Debug.Log($"[RangeUpdateStrategy] Объект {enemy.name} вошёл в радиус здания ({center}, radius={radius})");
                    }
                }
            }
            else
            {
                Debug.LogError("RangeUpdateStrategy: Неверный тип данных. Ожидался RangeModifierData.");
            }
        }
    }
}