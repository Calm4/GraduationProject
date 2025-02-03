namespace App.Scripts.Modifiers
{
    public interface IModifier
    {
        /// <summary>
        /// Метод выполнения логики модификатора в каждом кадре
        /// </summary>
        void UpdateModifier();
    }
}