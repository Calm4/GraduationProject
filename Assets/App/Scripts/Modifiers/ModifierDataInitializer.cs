using App.Scripts.Modifiers.Data;
using App.Scripts.Modifiers.Strategies;
using UnityEngine;

namespace App.Scripts.Modifiers
{
    public class ModifierDataInitializer : MonoBehaviour
    {
        private void Awake()
        {
            ModifierDataFactory.Register(ModifierType.AttackRate, (config) => new AttackRateModifierData());
            ModifierDataFactory.Register(ModifierType.Damage, (config) => new DamageModifierData());
            ModifierDataFactory.Register(ModifierType.Range, (config) => new RangeModifierData());


            ModifierUpdateStrategyFactory.Register(ModifierType.AttackRate, () => new AttackRateUpdateStrategy());
            ModifierUpdateStrategyFactory.Register(ModifierType.Damage, () => new DamageUpdateStrategy());
            ModifierUpdateStrategyFactory.Register(ModifierType.Range, () => new RangeUpdateStrategy());

            
            Debug.Log("ModifierDataFactory и ModifierUpdateStrategyFactory инициализированы.");
        }
    }
}