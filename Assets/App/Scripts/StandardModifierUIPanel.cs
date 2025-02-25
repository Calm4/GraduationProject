using System;
using System.Collections.Generic;
using App.Scripts.Buildings;
using UnityEngine;

namespace App.Scripts
{
    public class StandardModifierUIPanel : MonoBehaviour
    {
        [Header("Standard Modifiers Elements")]
        [SerializeField] private List<StandardModifierBuffUIInfo> standardModifiersPanels;
        
        private Building _parentBuilding;
        
        
        public void Initialize(Building parentBuilding)
        {
            _parentBuilding = parentBuilding;
            InitializeStandardModifiers();
        } 
        
        private void InitializeStandardModifiers()
        {
            
        }
    }
}