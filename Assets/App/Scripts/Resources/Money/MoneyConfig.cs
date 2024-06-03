using UnityEngine;

namespace App.Scripts.Resources.Money
{
    [CreateAssetMenu(fileName = "MoneyConfig", menuName = "Configs/Resources/MoneyConfig", order = 0)]
    public class MoneyConfig : ScriptableObject
    {
        public int currentAmount;
        public Sprite moneyImage;
    }
}