using System;
using System.Collections.Generic;
using UnityEngine;

namespace App.Scripts.Modifiers.Strategies
{
    public static class ModifierUpdateStrategyFactory
    {
        private static readonly Dictionary<ModifierType, Func<AbstractModifierUpdateStrategy>> StrategyMap = new();
        
        public static void Register(ModifierType modifierType, Func<AbstractModifierUpdateStrategy> strategyCreator)
        {
            if (!StrategyMap.ContainsKey(modifierType))
            {
                StrategyMap.Add(modifierType, strategyCreator);
            }
            else
            {
                Debug.LogWarning($"Стратегия обновления для типа {modifierType} уже зарегистрирована.");
            }
        }

        public static AbstractModifierUpdateStrategy Create(ModifierType modifierType)
        {
            if (StrategyMap.TryGetValue(modifierType, out var strategyCreator))
            {
                return strategyCreator.Invoke();
            }
            Debug.LogError($"Стратегия обновления для модификатора типа {modifierType} не зарегистрирована!");
            return null;
        }
    }
}