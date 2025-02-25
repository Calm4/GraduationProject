using System.Collections.Generic;
using App.Scripts.Buildings;
using UnityEngine;
using UnityEngine.Serialization;

namespace App.Scripts
{
    public class CustomModifierUIPanel : MonoBehaviour
    {
        [SerializeField] private CustomModifierPanelInInfoWindow firstLevelModifierPanel;
        [SerializeField] private CustomModifierPanelInInfoWindow secondLevelModifierPanel;
        [SerializeField] private CustomModifierPanelInInfoWindow thirdLevelModifierPanel;
        [SerializeField] private CustomModifierPanelInInfoWindow lastLevelModifierPanel;
        
        private Building _parentBuilding;
        
        public void Initialize(Building parentBuilding)
        {
            _parentBuilding = parentBuilding;
        }

    }
}