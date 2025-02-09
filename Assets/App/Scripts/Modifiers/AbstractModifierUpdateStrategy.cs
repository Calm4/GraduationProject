using App.Scripts.Buildings;
using App.Scripts.Modifiers.Data;

namespace App.Scripts.Modifiers
{
    public abstract class AbstractModifierUpdateStrategy
    {
        public void InitializeModifierUpdateStrategy(Building building)
        {
            OwnerBuilding = building;
        }
        public Building OwnerBuilding { get; private set; }
        public abstract void UpdateModifier(BaseModifierData data);
    }
}