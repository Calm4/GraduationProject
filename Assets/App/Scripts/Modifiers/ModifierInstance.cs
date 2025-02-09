using App.Scripts.Buildings;
using App.Scripts.Modifiers.Configs;
using App.Scripts.Modifiers.Data;
using App.Scripts.Modifiers.Strategies;

namespace App.Scripts.Modifiers
{
    public class ModifierInstance
    {
        public BaseModifierData ModifierData { get;}
        
        private readonly AbstractModifierUpdateStrategy _updateStrategy;
        
        public ModifierInstance(BaseModifierSO config, Building ownerBuilding = null)
        {
            ModifierData = ModifierDataFactory.Create(config);
            _updateStrategy = ModifierUpdateStrategyFactory.Create(config.modifierType);
            
            if (ownerBuilding != null)
            {
                _updateStrategy.InitializeModifierUpdateStrategy(ownerBuilding);
            }
        }
        
        public void UpdateModifier()
        {
            _updateStrategy?.UpdateModifier(ModifierData);
        }
    }
}