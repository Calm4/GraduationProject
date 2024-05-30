using System;

namespace App.Scripts.Resources
{
    [Serializable]
    public struct ResourceRequirement
    {
        public ResourceType resourceType;
        public int amount;
    }
}