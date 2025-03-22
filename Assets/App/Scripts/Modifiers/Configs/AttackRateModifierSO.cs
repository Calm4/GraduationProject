using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.Modifiers.Configs
{
    [CreateAssetMenu(fileName = "AttackRateModifier", menuName = "Configs/Modifiers/AttackRateModifier", order = 0)]
    public class AttackRateModifierSO : StandardModifiersSO
    {
        public float attackRate;
        
    }
}