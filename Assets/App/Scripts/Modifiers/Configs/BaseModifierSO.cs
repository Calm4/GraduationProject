using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.Modifiers.Configs
{
    public class BaseModifierSO : ScriptableObject
    {
        public string modifierName;
        public string modifierDescription;
        public Sprite modifierIcon;
        
        [Space(15)] [Header("Custom Modifiers")]
        public ModifierType modifierType;

    }
}