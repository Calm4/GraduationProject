using UnityEngine;
using Zenject;

namespace App.Scripts.GameResources
{
    public class ResourceDropVisualFactory : MonoBehaviour
    {
        [SerializeField] private GameObject resourceDropVisualPrefab;
        [SerializeField] private ResourceIconsConfig resourceIconsConfig;
        [SerializeField] private float randomOffsetRadius = 0.5f; // Радиус случайного смещения
        
        public void SpawnResourceDropVisual(Vector3 position, ResourceType resourceType, int amount)
        {
            // Добавляем случайное смещение по X и Z
            Vector3 randomOffset = new Vector3(
                Random.Range(-randomOffsetRadius, randomOffsetRadius),
                0f,
                Random.Range(-randomOffsetRadius, randomOffsetRadius)
            );
            
            var visual = Instantiate(resourceDropVisualPrefab, position + randomOffset, Quaternion.identity);
            var resourceDropVisual = visual.GetComponent<ResourceDropVisual>();
            var icon = resourceIconsConfig.GetIconForResource(resourceType);
            resourceDropVisual.Initialize(resourceType, amount, icon);
        }
    }
} 