using App.Scripts.Modifiers.Data;
using App.Scripts.Modifiers.Strategies;
using UnityEngine;

namespace App.Scripts.Modifiers
{
    /// <summary>
    /// Компонент для регистрации фабричных методов модификаторов.
    /// Его можно добавить в сцену (например, на GameManager) для глобальной инициализации.
    /// </summary>
    public class ModifierDataInitializer : MonoBehaviour
    {
        private void Awake()
        {
            // Регистрируем фабрики для всех типов модификаторов.
            // Если потребуется, можно также сделать ссылки на конкретные SO через [SerializeField],
            // но в данном примере регистрация происходит по ModifierType.
            ModifierDataFactory.Register(ModifierType.AttackRate, (config) => new AttackRateData());
            ModifierDataFactory.Register(ModifierType.Damage, (config) => new DamageData());

            ModifierUpdateStrategyFactory.Register(ModifierType.AttackRate, () => new AttackRateUpdateStrategy());
            ModifierUpdateStrategyFactory.Register(ModifierType.Damage, () => new DamageUpdateStrategy());
            
            Debug.Log("ModifierDataFactory и ModifierUpdateStrategyFactory инициализированы.");
        }
    }
}