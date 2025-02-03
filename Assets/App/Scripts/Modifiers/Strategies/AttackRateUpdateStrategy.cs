using App.Scripts.Modifiers.Data;
using UnityEngine;

namespace App.Scripts.Modifiers.Strategies
{
    public class AttackRateUpdateStrategy : IModifierUpdateStrategy
    {
        public void UpdateModifier(BaseModifierData data)
        {
            var attackData = data as AttackRateData;
            if (attackData != null)
            {
                // логика обновления для скорострельности.
                Debug.Log($"[AttackRateUpdateStrategy] Level: {attackData.currentLevel}, AttackRate: {attackData.currentAttackRate}");
            }
        }
    }
}