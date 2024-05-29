using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using System;

namespace App.Scripts.Resources
{
    public class ResourcesInitializer : SerializedMonoBehaviour
    {
        [Title("Resources Types",Bold = true)]
        [OdinSerialize] public Dictionary<ResourceType, ResourceData> resources;
        
       
    }
}