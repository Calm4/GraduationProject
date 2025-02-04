using App.Scripts.Modifiers.Configs;
using App.Scripts.Modifiers.Data;
using UnityEngine;

namespace App.Scripts.Modifiers.Strategies
{
    public class DamageUpdateStrategy : IModifierUpdateStrategy
    {
        public void UpdateModifier(BaseModifierData data)
        {
            var damageData = data as DamageData;
            if (damageData != null)
            {
                float baseDamage = 0f;
                if (damageData.Config is DamageModifierSO dmgConfig)
                {
                    baseDamage = dmgConfig.damage;
                }
                
                Debug.Log($"[DamageUpdateStrategy] Level: {damageData.currentLevel}, " +
                          $"Damage: {damageData.currentDamage}, BaseDamage from SO {damageData.Config.modifierName}: {baseDamage}");
            }
        }
    }
    
}