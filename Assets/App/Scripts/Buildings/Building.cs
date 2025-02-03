using App.Scripts.Buildings.BuildingsConfigs;
using App.Scripts.Modifiers;
using UnityEngine;

namespace App.Scripts.Buildings
{
    public class Building : MonoBehaviour
    {
        [field: SerializeField] public BasicBuildingConfig BuildingConfig { get; private set; }
        private ModifierManager _modifierManager;

        private void Awake()
        {
            _modifierManager = new ModifierManager(BuildingConfig);
        }

        private void Update()
        { 
            _modifierManager.UpdateModifiers();
        }
    }
}