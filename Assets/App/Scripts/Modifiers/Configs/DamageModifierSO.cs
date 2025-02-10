using UnityEngine;

namespace App.Scripts.Modifiers.Configs
{
    [CreateAssetMenu(fileName = "DamageModifier", menuName = "Configs/Modifiers/DamageModifier", order = 0)]
    public class DamageModifierSO : BaseModifierSO
    {
        public int damage;
    }
}