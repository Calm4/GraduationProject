using System;
using System.Collections.Generic;
using App.Scripts.Buildings;
using UnityEngine;

namespace App.Scripts.Modifiers
{
    public class ModifierManager
    {
        private List<Modifier> _modifiers;

        public List<Modifier> GetCurrentModifiers()
        {
            return _modifiers;
        }

        
        public void ApplyModifier(Modifier modifier)
        {
            _modifiers.Add(modifier);
        }

        public void RemoveModifier(Modifier modifier)
        {
            _modifiers.Remove(modifier);
        }
        
        public void UpdateModifiers()
        {
            foreach (Modifier modifier in _modifiers)
            {
                modifier.ModifierUpdate();
            }
        }

        public void Log_GetFullModifiersList()
        {
            foreach (Modifier modifier in _modifiers)
            {
                Debug.Log(modifier.BaseModifierSO.modifierName);
            }
        }
    }
}