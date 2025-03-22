using System.Collections.Generic;
using App.Scripts.Modifiers.Configs;
using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.Modifiers
{
    [CreateAssetMenu(fileName = "ModifiersDataBase", menuName = "Configs/DataBases/ModifiersDataBase", order = 0)]    
    public class ModifiersDataBase : SerializedScriptableObject
    {
        [DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.Foldout)]
        public readonly Dictionary<ModifierType, BaseModifierSO> ModifierConfigs = new();
        
    }
}