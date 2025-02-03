using System;
using App.Scripts.Modifiers.Configs;
using UnityEngine;

namespace App.Scripts.Modifiers.Data
{
    [Serializable]
    public class DamageData : BaseModifierData
    {
        public float currentDamage;

        public override void ResetToDefault(BaseModifierSO config)
        {
            if (config is DamageModifierSO damageConfig)
            {
                currentDamage = damageConfig.damage;
            }
            else
            {
                Debug.LogError("Unexpected type in Data!");
            }
        }
    }
}