using Sirenix.OdinInspector;
using UnityEngine;

namespace App.Scripts.Buildings.UI.BuildingButtons.Configs
{
    [CreateAssetMenu(fileName = "BuildingButtonConfig", menuName = "Configs/Gameplay Objects/Buildings/BuildingButtonConfig", order = 1)]
    public class BuildingButtonSO : SerializedScriptableObject
    {
        [Title("Button Data")]
        public string buttonTitle;
        public Sprite buttonIcon;
        
        [Title("UI Panel to Open")]
        [Tooltip("Префаб UI панели, который будет открываться при нажатии на кнопку")]
        public GameObject uiPanelPrefab;
    }
}