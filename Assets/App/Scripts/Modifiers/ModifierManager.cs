using System;
using System.Collections.Generic;
using App.Scripts.Buildings;
using App.Scripts.Buildings.BuildingsConfigs;
using App.Scripts.Modifiers.Configs;
using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.Modifiers
{
    public class ModifierManager
    {
        [ShowInInspector, DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.Foldout)]
        private Dictionary<ModifierType, ModifierInstance> _modifierInstances = new Dictionary<ModifierType, ModifierInstance>();

        private readonly ModifiersDataBase _modifiersDataBase;
        
        public ModifierManager(BasicBuildingConfig buildingConfig, ModifiersDataBase modifiersDataBase)
        {
            _modifiersDataBase = modifiersDataBase;
            InitializeBaseModifiers(buildingConfig);
        }
        
        /// <summary>
        /// Инициализирует базовые модификаторы, заданные в конфигурации здания.
        /// Если модификатор данного типа уже добавлен – повторное добавление игнорируется.
        /// </summary>
        private void InitializeBaseModifiers(BasicBuildingConfig buildingConfig)
        {
            foreach (var baseMod in buildingConfig.initialModifiers)
            {
                if (!_modifierInstances.ContainsKey(baseMod.modifierType))
                {
                    _modifierInstances.Add(baseMod.modifierType, new ModifierInstance(baseMod));
                }
                else
                {
                    Debug.Log($"Модификатор типа {baseMod.modifierType} уже существует – повторное добавление игнорируется.");
                }
            }
        }
        
        /// <summary>
        /// Добавляет новый модификатор, если его типа ещё нет.
        /// </summary>
        public void ApplyModifier(ModifierType modifierType)
        {
            if (!_modifierInstances.ContainsKey(modifierType))
            {
                if (_modifiersDataBase.ModifierConfigs.TryGetValue(modifierType, out BaseModifierSO config))
                {
                    _modifierInstances.Add(modifierType, new ModifierInstance(config));
                }
                else
                {
                    Debug.LogError($"Конфигурация модификатора для типа {modifierType} не найдена в репозитории!");
                }
            }
            else
            {
                //TODO: Возможно тут сделать что модификатор становится второго уровня и теперь нас усиленный модификатор ???
                Debug.Log($"Модификатор типа {modifierType} уже присутствует – применение не требуется.");
            }
        }
        
        /// <summary>
        /// Удаляет модификатор по типу.
        /// </summary>
        public void RemoveModifier(ModifierType modType)
        {
            if (_modifierInstances.ContainsKey(modType))
            {
                _modifierInstances.Remove(modType);
            }
        }
        
        public Dictionary<ModifierType, ModifierInstance> GetCurrentModifiers()
        {
            return _modifierInstances;
        }
        
        /// <summary>
        /// Обновляет все модификаторы.
        /// </summary>
        public void UpdateModifiers()
        {
            foreach (var modifier in _modifierInstances.Values)
            {
                modifier.UpdateModifier();
            }
        }

        public void Log_GetFullModifiersList()
        {
            foreach (var modifier in _modifierInstances.Values)
            {
                Debug.Log(modifier.Config.modifierName);
            }
        }
        
        public List<BaseModifierSO> GetActiveModifiers()
        {
            List<BaseModifierSO> activeModifiers = new List<BaseModifierSO>();
            foreach (var modifier in _modifierInstances.Values)
            {
                activeModifiers.Add(modifier.Config);
            }
            return activeModifiers;
        }

    }
}