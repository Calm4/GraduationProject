using System;
using App.Scripts.Modifiers.Configs;
using UnityEngine;

namespace App.Scripts.Modifiers.Data
{
    [Serializable]
    public class AttackRateData : BaseModifierData
    {
        public float currentAttackRate;

        public override void ResetToDefault(BaseModifierSO config)
        {
            if (config is AttackRateModifierSO attackConfig)
            {
                currentAttackRate = attackConfig.attackRate;
            }
            else
            {
                Debug.LogError("Unexpected type in Data!");
            }
        }
    }
}