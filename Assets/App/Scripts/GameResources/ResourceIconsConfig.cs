using UnityEngine;
using System.Collections.Generic;
using App.Scripts.GameResources;

namespace App.Scripts.GameResources
{
    [CreateAssetMenu(fileName = "ResourceIconsConfig", menuName = "Configs/Gameplay Objects/ResourceIcons", order = 0)]
    public class ResourceIconsConfig : ScriptableObject
    {
        [System.Serializable]
        public class ResourceIconData
        {
            public ResourceType resourceType;
            public Sprite icon;
        }

        public List<ResourceIconData> resourceIcons = new List<ResourceIconData>();

        public Sprite GetIconForResource(ResourceType resourceType)
        {
            foreach (var resourceIcon in resourceIcons)
            {
                if (resourceIcon.resourceType == resourceType)
                {
                    return resourceIcon.icon;
                }
            }
            return null;
        }
    }
} 