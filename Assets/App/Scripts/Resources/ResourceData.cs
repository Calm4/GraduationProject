using System;

namespace App.Scripts.Resources
{
    [Serializable]
    public struct ResourceData
    {
        public ResourceConfig resourceConfig;
        public int currentAmount;
        public int maxAmount;
        public bool isUnlockedNow;
    }
}
