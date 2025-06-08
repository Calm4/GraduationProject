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
        private BuildingRangeVisualizer _buildingRangeVisualizer;


        public ModifierManager ModifierManager => _modifierManager;

        [Button]
        public void AddModifier()
        {
            _modifierManager.ApplyModifier(ModifierType.Damage);
        }

        [ShowInInspector, ReadOnly]
        public Dictionary<ModifierType, ModifierInstance> StandardModifiers =>
            _modifierManager?.GetCurrentModifiers() ?? new Dictionary<ModifierType, ModifierInstance>();

        [ShowInInspector, ReadOnly] public Dictionary<ModifierType, ModifierInstance> CustomModifiers => new();

        private void Awake()
        {
            _modifierManager = new ModifierManager(BuildingConfig, this, modifiersDataBase);


            _buildingRangeVisualizer = GetComponent<BuildingRangeVisualizer>();
            if (_buildingRangeVisualizer != null)
            {
                _buildingRangeVisualizer.Initialize(this);
            }
        }

        private void Update()
        {
            _modifierManager.UpdateModifiers();
        }

        private void OnMouseDown()
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            Debug.Log("Clicked on: " + this.gameObject.name);
            if (BuildingConfig.buildingType == BuildingType.NonInteractive)
                return;

            OnBuildingClicked?.Invoke(this);

            if (_buildingRangeVisualizer != null)
            {
                _buildingRangeVisualizer.ShowVisualizer();
            }
        }


        public static event Action<Building> OnBuildingClicked;
    }
}