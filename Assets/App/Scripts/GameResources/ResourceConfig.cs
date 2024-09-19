using UnityEngine;

namespace App.Scripts.GameResources
{
    [CreateAssetMenu(fileName = "_ResourceConfig", menuName = "Configs/Resources/SpecificResource", order = 0)]
    public class ResourceConfig : BasicResourceConfig
    {
        public int buyCost;
        public int saleCost;
        public int initialMaxAmount;
        
    }
}