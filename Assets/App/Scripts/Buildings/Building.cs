using System.Collections.Generic;
using App.Scripts.Buildings.BuildingsConfigs;
using App.Scripts.Modifiers;
using App.Scripts.Modifiers.Configs;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace App.Scripts.Buildings
{
    public class Building : MonoBehaviour
    {
        [SerializeField] private ModifiersDataBase modifiersDataBase;
        [field: SerializeField] public BasicBuildingConfig BuildingConfig { get; private set; }
        private ModifierManager _modifierManager;

        [ShowInInspector, ReadOnly]
        public List<BaseModifierSO> ActiveModifiers
        {
            get
            {
                if (_modifierManager != null)
                {
                    return _modifierManager.GetActiveModifiers();
                }
                return new List<BaseModifierSO>();
            }
        }
        
        private void Awake()
        {
            _modifierManager = new ModifierManager(BuildingConfig, modifiersDataBase);
        }

        private void Update()
        { 
            _modifierManager.UpdateModifiers();
        }
        
        [Button("Добавить AttackRate модификатор")]
        private void AddAttackRateModifier()
        {
            _modifierManager.ApplyModifier(ModifierType.AttackRate);
        }
    }
}