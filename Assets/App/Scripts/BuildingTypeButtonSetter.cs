using App.Scripts;
using UnityEngine;
using UnityEngine.UI;
using App.Scripts.Buildings.BuildingsConfigs;

[RequireComponent(typeof(Button))]
public class BuildingTypeButtonSetter : MonoBehaviour
{
    [SerializeField] private BuildingTypeVisibilityManager visibilityManager;
    [SerializeField] private BuildingType buildingType;  // Выбор типа из enum

    private void Start()
    {
        var button = GetComponent<Button>();
        if (button != null && visibilityManager != null)
        {
            // Добавляем событие на кнопку
            button.onClick.AddListener(() => visibilityManager.ShowBuildingType(buildingType));
        }
    }
}