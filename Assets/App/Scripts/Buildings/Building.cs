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
        private RangeVisualizer _rangeVisualizer;


        [ShowInInspector, ReadOnly]
        public Dictionary<ModifierType, ModifierInstance> ActiveModifiers =>
            _modifierManager?.GetCurrentModifiers() ?? new Dictionary<ModifierType, ModifierInstance>();

        private void Awake()
        {
            _modifierManager = new ModifierManager(BuildingConfig, this, modifiersDataBase);
           
            _rangeVisualizer = GetComponent<RangeVisualizer>();
            _rangeVisualizer.Initialize(this);

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
            
            if (_rangeVisualizer != null)  
                _rangeVisualizer.ShowVisualizer();
        }

       
        
        public static event Action<Building> OnBuildingClicked;
    }
}