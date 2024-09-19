using System;

namespace App.Scripts.GameResources
{
    [Serializable]
    public struct IncomingResources
    {
        public ResourceType resourceType;
        public int amount;
    }
}