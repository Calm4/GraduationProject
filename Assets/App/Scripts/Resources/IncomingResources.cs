using System;
using UnityEngine;

namespace App.Scripts.Resources
{
    [Serializable]
    public struct IncomingResources
    {
        public ResourceType resourceType;
        public int amount;
    }
}