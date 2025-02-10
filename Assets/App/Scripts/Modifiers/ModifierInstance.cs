using App.Scripts.Buildings;
using App.Scripts.Modifiers.Configs;
using App.Scripts.Modifiers.Data;
using App.Scripts.Modifiers.Strategies;

namespace App.Scripts.Modifiers
{
    public class ModifierInstance
    {
        public BaseModifierData ModifierData { get; }
        public AbstractModifierUpdateStrategy UpdateStrategy { get; }

        public ModifierInstance(BaseModifierSO config, Building ownerBuilding = null, ModifierManager modifierManager = null)
        {
            ModifierData = ModifierDataFactory.Create(config);
            UpdateStrategy = ModifierUpdateStrategyFactory.Create(config.modifierType);

            if (ownerBuilding != null && modifierManager != null)
            {
                UpdateStrategy.InitializeModifierUpdateStrategy(ownerBuilding, modifierManager);
            }
        }

        public void UpdateModifier()
        {
            UpdateStrategy?.UpdateModifier(ModifierData);
        }
    }
}