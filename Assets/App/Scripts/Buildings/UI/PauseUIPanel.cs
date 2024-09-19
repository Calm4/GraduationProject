using App.Scripts.GameResources;
using TMPro;
using UnityEngine;

namespace App.Scripts.Buildings.UI
{
    public class PauseUIPanel : MonoBehaviour
    {
        [SerializeField] private ResourcesManager resourceManager;
        [SerializeField] private TMP_Text moneyTextField;
    
        void Start()
        {
            moneyTextField.text = resourceManager.GetResourceData(ResourceType.Money).currentAmount.ToString();
        }
    }
}
