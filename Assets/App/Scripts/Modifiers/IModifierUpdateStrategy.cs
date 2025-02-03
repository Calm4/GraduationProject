using App.Scripts.Modifiers.Data;

namespace App.Scripts.Modifiers
{
    /// <summary>
    /// Интерфейс, описывающий стратегию обновления для модификатора.
    /// </summary>
    public interface IModifierUpdateStrategy
    {
        void UpdateModifier(BaseModifierData data);
    }
}