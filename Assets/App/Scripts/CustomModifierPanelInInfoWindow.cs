using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts
{
    public class CustomModifierPanelInInfoWindow : BaseModifierPanelInInfoWindow
    {
        [SerializeField] private Image modifierImage;
        
        [SerializeField] private TMP_Text modifierName;
        
        [SerializeField] private List<ModifierBuffUIInfo> modifierBuffs;
    }
}