using System;
using System.Collections.Generic;
using App.Scripts.Buildings.BuildingsConfigs;
using App.Scripts.Modifiers;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace App.Scripts.Buildings
{
    public class Building : MonoBehaviour
    {
        [SerializeField] private List<Modifier> modifiers;
        [SerializeField] private ModifierManager modifierManager;
        [field: SerializeField] public BasicBuildingConfig BuildingConfig { get; private set; }

        private void Awake()
        {
            modifierManager = new ModifierManager();
        }


        private void Update()
        {
            modifierManager.UpdateModifiers();
        }
    }
}