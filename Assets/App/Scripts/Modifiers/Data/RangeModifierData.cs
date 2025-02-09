using App.Scripts.Modifiers.Configs;
using UnityEngine;

namespace App.Scripts.Modifiers.Data
{
    public class RangeModifierData : BaseModifierData
    {
        public float currentRange;

        public override void ResetToDefault(BaseModifierSO config)
        {
            if (config is RangeModifierSO rangeConfig)
            {
                currentRange = rangeConfig.range;
            }
            else
            {
                Debug.LogError("Unexpected type in RangeData ResetToDefault. Ожидался RangeModifierSO!");
            }
        }
    }
}