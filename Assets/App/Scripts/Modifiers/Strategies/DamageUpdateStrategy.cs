using App.Scripts.Modifiers.Data;

namespace App.Scripts.Modifiers.Strategies
{
    public class DamageUpdateStrategy : IModifierUpdateStrategy
    {
        public void UpdateModifier(BaseModifierData data)
        {
            var damageData = data as DamageData;
            if (damageData != null)
            {
                // Здесь реализуем логику обновления для урона.
                UnityEngine.Debug.Log($"[DamageUpdateStrategy] Level: {damageData.currentLevel}, Damage: {damageData.currentDamage}");
            }
        }
    }
}