using System;
using System.Collections.Generic;
using App.Scripts.Buildings;
using App.Scripts.Modifiers;
using App.Scripts.Modifiers.Data;
using TMPro;
using UnityEngine;

namespace App.Scripts
{
    public class StandardModifierUIPanel : MonoBehaviour
    {
        [Header("Standard Modifiers Elements")]
        [SerializeField] private List<StandardModifierBuffUIInfo> standardModifiersPanels = new();
        
        private Building _parentBuilding;
        
        
        public void Initialize(Building parentBuilding)
        {
            _parentBuilding = parentBuilding;
            InitializeStandardModifiers();
        } 
        
        private void InitializeStandardModifiers()
        {
            var modifierPanels = new Dictionary<Type, int>
            {
                { typeof(AttackRateModifierData), 0 },
                { typeof(RangeModifierData), 1 },
                { typeof(DamageModifierData), 2 }
            };

            foreach (var modifier in _parentBuilding.StandardModifiers.Values)
            {
                if (modifierPanels.TryGetValue(modifier.ModifierData.GetType(), out int panelIndex))
                {
                    var textComponent = standardModifiersPanels[panelIndex].GetComponentInChildren<TMP_Text>();

                    switch (modifier.ModifierData)
                    {
                        case AttackRateModifierData attackData:
                            textComponent.text = attackData.currentAttackRate.ToString();
                            break;
                        case RangeModifierData rangeData:
                            textComponent.text = rangeData.currentRange.ToString();
                            break;
                        case DamageModifierData damageData:
                            textComponent.text = damageData.currentDamage.ToString();
                            break;
                    }
                }
            }
        }

    }
}