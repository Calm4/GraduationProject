using System;

namespace App.Scripts.GameResources
{
    [Serializable]
    public struct ResourceRequirement
    {
        public int amount;
        public BasicResourceConfig config;
    }
}