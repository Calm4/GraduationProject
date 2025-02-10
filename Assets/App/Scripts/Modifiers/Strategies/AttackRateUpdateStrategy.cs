using App.Scripts.Buildings;
using App.Scripts.Enemies;
using App.Scripts.Modifiers.Data;
using UnityEngine;

namespace App.Scripts.Modifiers.Strategies
{
    public class AttackRateUpdateStrategy : AbstractModifierUpdateStrategy
    {
        private float lastAttackTime = 0f;

        public override void UpdateModifier(BaseModifierData data)
        {
            if (data is AttackRateModifierData attackRateData)
            {
                if (OwnerBuilding == null)
                {
                    Debug.LogError("AttackRateUpdateStrategy: OwnerBuilding не установлен!");
                    return;
                }

                float attackRate = attackRateData.currentAttackRate;
                if (Time.time - lastAttackTime >= 1f / attackRate)
                {
                    ShootAtEnemy();
                    lastAttackTime = Time.time;
                }
            }
            else
            {
                Debug.LogError("AttackRateUpdateStrategy: Неверный тип данных. Ожидался AttackRateModifierData.");
            }
        }

        private void ShootAtEnemy()
        {
            var rangeInstance = ModifierManager.GetCurrentModifiers()[ModifierType.Range];
            var rangeStrategy = (RangeUpdateStrategy)rangeInstance.UpdateStrategy;
            Enemy targetEnemy = rangeStrategy.GetTargetEnemy();

            if (targetEnemy != null)
            {
                FireBullet(targetEnemy);
            }
        }

        private void FireBullet(Enemy enemy)
        {
            var damageInstance = ModifierManager.GetCurrentModifiers()[ModifierType.Damage];
            var damageData = (DamageModifierData)damageInstance.ModifierData;
            int damage = damageData.currentDamage;

            enemy.TakeDamage(damage);
            Debug.Log($"[AttackRateUpdateStrategy] Атака на {enemy.name} нанесла {damage} урона.");
        }
    }
}
