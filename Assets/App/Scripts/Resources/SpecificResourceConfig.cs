using UnityEngine;

namespace App.Scripts.Resources
{
    [CreateAssetMenu(fileName = "_ResourceConfig", menuName = "Configs/Resources/SpecificResource", order = 0)]
    public class SpecificResourceConfig : ScriptableObject
    {
        public ResourcesTypes resourceType;
        public int buyCost;
        public int saleCost;
    }
}