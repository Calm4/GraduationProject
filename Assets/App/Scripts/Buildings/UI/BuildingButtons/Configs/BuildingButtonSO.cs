using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.Buildings.UI.BuildingButtons.Configs
{
    [CreateAssetMenu(fileName = "_BuildingButtonConfig", menuName = "Configs/User Interface/Building Buttons/BuildingButtonConfig", order = 1)]
    public class BuildingButtonSO : SerializedScriptableObject
    {
        [Title("Button Data")]
        public string buttonTitle;
        public Sprite buttonIcon;
        
        [Title("UI Panel to Open")]
        public RectTransform uiPanelPrefab;
    }
}