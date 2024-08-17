using App.Scripts.Buildings;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts
{
    public class HideAndShowBuildingsUIPanel : MonoBehaviour
    {
        [SerializeField] private Image panelImage;
    
        [SerializeField] private BuildingPanelUI buildingPanelUI;

        private void Start()
        {
            buildingPanelUI.OnButtonPressed += BuildingUIControllerOnButtonPressed;
        }

        private void BuildingUIControllerOnButtonPressed(float animationRotateTime)
        {
            panelImage.transform.DORotate(new Vector3(180, 0, 0), animationRotateTime, RotateMode.LocalAxisAdd);
        }
    }
}
