﻿using UnityEngine;

namespace App.Scripts.Resources
{
    [CreateAssetMenu(fileName = "_ResourceConfig", menuName = "Configs/Resources/SpecificResource", order = 0)]
    public class ResourceConfig : ScriptableObject
    {
        public ResourceType resourceType;
        public int buyCost;
        public int saleCost;
        public int initialMaxAmount;
    }
}