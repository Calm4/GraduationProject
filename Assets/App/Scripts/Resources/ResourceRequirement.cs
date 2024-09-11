using System;
using UnityEngine.Serialization;

namespace App.Scripts.Resources
{
    [Serializable]
    public struct ResourceRequirement
    {
        public int amountToBuild;
        public ResourceConfig resourceConfig;
    }
}