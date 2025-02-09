using System;
using App.Scripts.Modifiers.Configs;

namespace App.Scripts.Modifiers.Data
{
    [Serializable]
    public abstract class BaseModifierData
    {
        public int currentLevel = 1;
        
        public BaseModifierSO Config { get; set; }
        
        public abstract void ResetToDefault(BaseModifierSO config);
    }
}