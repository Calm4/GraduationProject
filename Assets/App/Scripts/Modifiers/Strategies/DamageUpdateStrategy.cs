using App.Scripts.Modifiers.Data;
using UnityEngine;

namespace App.Scripts.Modifiers.Strategies
{
    public class DamageUpdateStrategy : AbstractModifierUpdateStrategy
    {
        public override void UpdateModifier(BaseModifierData data)
        {
            if (data is DamageModifierData damageData)
            {
                if (OwnerBuilding == null)
                {
                    Debug.LogError("DamageUpdateStrategy: OwnerBuilding не установлен!");
                    return;
                }

                float damage = damageData.currentDamage;
                //Debug.Log($"[DamageUpdateStrategy] Урон установлен: {damage}");
            }
            else
            {
                Debug.LogError("DamageUpdateStrategy: Неверный тип данных. Ожидался DamageModifierData.");
            }
        }
    }
}