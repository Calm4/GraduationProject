using App.Scripts.Buildings;
using App.Scripts.Enemies;
using App.Scripts.Modifiers.Data;
using UnityEngine;

namespace App.Scripts.Modifiers.Strategies
{
    public class RangeUpdateStrategy : AbstractModifierUpdateStrategy
    {

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
                foreach (Collider hit in hits)
                {
                    Enemy enemy = hit.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        Debug.Log($"[RangeUpdateStrategy] Уничтожен {enemy.name}");
                        GameObject.Destroy(enemy.gameObject);
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