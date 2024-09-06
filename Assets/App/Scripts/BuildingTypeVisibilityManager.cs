using System.Collections.Generic;
using UnityEngine;
using App.Scripts.Buildings.BuildingsConfigs;

namespace App.Scripts
{
    public class BuildingTypeVisibilityManager : MonoBehaviour
    {
        [SerializeField] private List<Transform> buildingTypeContainers; // Список всех контейнеров с BuildingTypeComponent

        // Метод принимает тип BuildingType напрямую
        public void ShowBuildingType(BuildingType selectedType)
        {
            foreach (var container in buildingTypeContainers)
            {
                var typeComponent = container.GetComponent<BuildingTypeComponent>();
                if (typeComponent != null)
                {
                    // Включаем контейнер только если тип здания совпадает с выбранным
                    container.gameObject.SetActive(typeComponent.BuildingType == selectedType);
                }
            }
        }
    }
}