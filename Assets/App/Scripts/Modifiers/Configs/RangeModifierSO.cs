using UnityEngine;

namespace App.Scripts.Modifiers.Configs
{
    [CreateAssetMenu(fileName = "RangeModifier", menuName = "Configs/Modifiers/RangeModifier", order = 1)]
    public class RangeModifierSO : StandardModifiersSO
    {
        public float range;
    }
}