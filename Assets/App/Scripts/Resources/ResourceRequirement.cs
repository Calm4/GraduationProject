using System;
using UnityEngine.Serialization;

namespace App.Scripts.Resources
{
    [Serializable]
    public struct ResourceRequirement
    {
        public ResourceType resourceType;
        public int amountToBuild;
    }
}