using UnityEngine;

namespace App.Scripts.GameResources
{
    [CreateAssetMenu(fileName = "_MaterialResource", menuName = "Configs/Resources/MaterialResource", order = 0)]
    public class MaterialResourceConfig : BasicResourceConfig
    {
        public int buyCost;
        public int saleCost;
        public int initialMaxAmount;
        
    }
}