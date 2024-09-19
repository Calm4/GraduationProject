using System;

namespace App.Scripts.GameResources
{
    [Serializable]
    public struct ResourceData
    {
        public BasicResourceConfig resourceConfig;
        public int currentAmount;
        public int maxAmount;
        public bool isUnlockedNow;
    }
}
