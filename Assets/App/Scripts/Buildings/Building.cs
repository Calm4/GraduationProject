using System;
using System.Collections.Generic;
using App.Scripts.Buildings.BuildingsConfigs;
using App.Scripts.Modifiers;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

namespace App.Scripts.Buildings
{
    public class Building : MonoBehaviour
    {
        [SerializeField] private ModifiersDataBase modifiersDataBase;
        [field: SerializeField] public BasicBuildingConfig BuildingConfig { get; private set; }
        private ModifierManager _modifierManager;

        [ShowInInspector, ReadOnly]
        public Dictionary<ModifierType, ModifierInstance> ActiveModifiers =>
            _modifierManager?.GetCurrentModifiers() ?? new Dictionary<ModifierType, ModifierInstance>();
        
        private void Awake()
        {
            // Передаём ссылку на текущее здание (this)
            _modifierManager = new ModifierManager(BuildingConfig, this, modifiersDataBase);
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

        private void OnMouseDown()
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            Debug.Log("Clicked on: " + this.gameObject.name);
            if (this.BuildingConfig.buildingType == BuildingType.NonInteractive)
                return;
            
            OnBuildingClicked?.Invoke(this);
        }

        public static event Action<Building> OnBuildingClicked;
    }
}
