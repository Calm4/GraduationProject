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
        private readonly Building _ownerBuilding;

        public ModifierManager(BasicBuildingConfig buildingConfig, Building ownerBuilding, ModifiersDataBase modifiersDataBase)
        {
            _modifiersDataBase = modifiersDataBase;
            _ownerBuilding = ownerBuilding;
            InitializeBaseModifiers(buildingConfig);
        }

        private void InitializeBaseModifiers(BasicBuildingConfig buildingConfig)
        {
            foreach (var baseMod in buildingConfig.initialModifiers)
            {
                if (!_modifierInstances.ContainsKey(baseMod.modifierType))
                {
                    _modifierInstances.Add(baseMod.modifierType, new ModifierInstance(baseMod, _ownerBuilding, this));
                }
                else
                {
                    Debug.Log($"Модификатор типа {baseMod.modifierType} уже существует – повторное добавление игнорируется.");
                }
            }
        }

        public void ApplyModifier(ModifierType modifierType)
        {
            if (!_modifierInstances.ContainsKey(modifierType))
            {
                if (_modifiersDataBase.ModifierConfigs.TryGetValue(modifierType, out BaseModifierSO config))
                {
                    _modifierInstances.Add(modifierType, new ModifierInstance(config, _ownerBuilding, this));
                }
                else
                {
                    Debug.LogError($"Конфигурация модификатора для типа {modifierType} не найдена в репозитории!");
                }
            }
            else
            {
                Debug.Log($"Модификатор типа {modifierType} уже присутствует – применение не требуется.");
            }
        }

        public Dictionary<ModifierType, ModifierInstance> GetCurrentModifiers() => _modifierInstances;

        public void UpdateModifiers()
        {
            foreach (var modifier in _modifierInstances.Values)
            {
                modifier.UpdateModifier();
            }
        }
    }
}
