using App.Scripts.Buildings;
using App.Scripts.Modifiers.Data;
using UnityEngine;

namespace App.Scripts.Modifiers.Strategies
{
    public class AttackRateUpdateStrategy : AbstractModifierUpdateStrategy
    {
        public override void UpdateModifier(BaseModifierData data)
        {
            var attackData = data as AttackRateModifierData;
            if (attackData != null)
            {
                // логика обновления для скорострельности.
                //Debug.Log($"[AttackRateUpdateStrategy] Level: {attackData.currentLevel}, AttackRate: {attackData.currentAttackRate}");
            }
        }
    }
}