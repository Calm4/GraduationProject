using System;
using App.Scripts.Modifiers.Configs;

namespace App.Scripts.Modifiers
{
    public abstract class Modifier
    {
        public BaseModifierSO BaseModifierSO { get; private set; }
        public abstract void ModifierUpdate();
    }
}