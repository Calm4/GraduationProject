using App.Scripts.Buildings;
using App.Scripts.Modifiers.Data;

namespace App.Scripts.Modifiers
{
    public abstract class AbstractModifierUpdateStrategy
    {
        public Building OwnerBuilding { get; private set; }
        public ModifierManager ModifierManager { get; private set; }

        public void InitializeModifierUpdateStrategy(Building building, ModifierManager modifierManager)
        {
            OwnerBuilding = building;
            ModifierManager = modifierManager;
        }

        public abstract void UpdateModifier(BaseModifierData data);
    }
}