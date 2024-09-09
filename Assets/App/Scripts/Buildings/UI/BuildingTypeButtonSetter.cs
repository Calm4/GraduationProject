using App.Scripts.Buildings.BuildingsConfigs;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Buildings.UI
{
    [RequireComponent(typeof(Button))]
    public class BuildingTypeButtonSetter : MonoBehaviour
    {
        [SerializeField] private BuildingTypeVisibilityManager visibilityManager;
        [SerializeField] private BuildingType buildingType;
        [SerializeField] private Transform buildingsContainer;

        private void Start()
        {
            var button = GetComponent<Button>();
            if (button != null && visibilityManager != null)
            {
                button.onClick.AddListener(() => visibilityManager.ShowBuildingType(buildingType));
            }
        }
    }
}