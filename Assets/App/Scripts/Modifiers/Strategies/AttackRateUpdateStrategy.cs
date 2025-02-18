using App.Scripts.Buildings;
using App.Scripts.Buildings.BuildingsConfigs;
using App.Scripts.Enemies;
using App.Scripts.Modifiers.Data;
using App.Scripts.Projectiles;
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

            var config = (DefensiveBuildingConfig)OwnerBuilding.BuildingConfig;
            if (config == null || config.projectilePrefab == null)
            {
                Debug.LogError("Префаб снаряда не назначен в BuildingConfig!");
                return;
            }
            
            Vector3 spawnPosition = OwnerBuilding.transform.position + new Vector3(0,2,0);
            Projectile projectileObj = GameObject.Instantiate(config.projectilePrefab, spawnPosition, Quaternion.identity);
            Projectile projectile = projectileObj.GetComponent<Projectile>();
            if (projectile != null)
            {
                projectile.Initialize(enemy, damage);
            }
            else
            {
                Debug.LogError("Компонент Projectile не найден на префабе снаряда!");
            }
        }
    }
}
