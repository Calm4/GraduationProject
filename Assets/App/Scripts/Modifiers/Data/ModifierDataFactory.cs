using System;
using System.Collections.Generic;
using App.Scripts.Modifiers.Configs;
using UnityEngine;

namespace App.Scripts.Modifiers.Data
{
    public static class ModifierDataFactory
    {
        private static readonly Dictionary<ModifierType, Func<BaseModifierSO, BaseModifierData>> FactoryMap = new();
        
        /// <summary>
        /// Регистрирует фабричный метод для данного типа модификатора.
        /// Вызывается один раз, например, при инициализации игры или через статический конструктор.
        /// </summary>
        public static void Register(ModifierType modifierType, Func<BaseModifierSO, BaseModifierData> factoryMethod)
        {
            if (!FactoryMap.ContainsKey(modifierType))
            {
                FactoryMap.Add(modifierType, factoryMethod);
            }
        }
        
        /// <summary>
        /// Создает объект данных для указанного SO, используя зарегистрированную фабрику.
        /// </summary>
        public static BaseModifierData Create(BaseModifierSO config)
        {
            if (FactoryMap.TryGetValue(config.modifierType, out var factoryMethod))
            {
                var data = factoryMethod.Invoke(config);
                data.Config = config;
                data.ResetToDefault(config);
                return data;
            }
            Debug.LogError($"Не найдена фабрика для модификатора типа {config.modifierType}");
            return null;
        }
    }
}