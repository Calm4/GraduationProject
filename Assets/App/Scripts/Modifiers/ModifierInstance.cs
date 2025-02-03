using System;
using App.Scripts.Modifiers.Configs;
using App.Scripts.Modifiers.Data;
using App.Scripts.Modifiers.Strategies;
using UnityEngine;

namespace App.Scripts.Modifiers
{
    public class ModifierInstance : IModifier
    {
        public BaseModifierSO Config { get; private set; }
        public BaseModifierData Data { get; private set; }
        
        // Стратегия обновления, отвечающая за выполнение индивидуальной логики.
        private IModifierUpdateStrategy _updateStrategy;

        public ModifierInstance(BaseModifierSO config)
        {
            Config = config;
            Data = ModifierDataFactory.Create(config);
            _updateStrategy = ModifierUpdateStrategyFactory.Create(config.modifierType);
        }
        
        /// <summary>
        /// Метод обновления, который делегирует обновление стратегии.
        /// </summary>
        public void UpdateModifier()
        {
            _updateStrategy?.UpdateModifier(Data);
        }
        
    }
}