using System;
using App.Scripts.Modifiers.Configs;

namespace App.Scripts.Modifiers.Data
{
    [Serializable]
    public abstract class BaseModifierData
    {
        public int currentLevel = 1;

        /// <summary>
        /// Метод для сброса данных к значениям по умолчанию (на основе конфигурации).
        /// </summary>
        public abstract void ResetToDefault(BaseModifierSO config);
    }
}