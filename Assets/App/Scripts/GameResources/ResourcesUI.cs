using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.GameResources
{
    public class ResourcesUI : MonoBehaviour
    {
        [SerializeField] private ResourcesDataBase resourcesDataBase;
        [SerializeField] private Transform resourceUIContainer;
        [SerializeField] private ResourcesManager resourcesManager;
        [SerializeField] private GameObject resourceUIPrefab;

        private readonly Dictionary<ResourceType, GameObject> _resourceUIElements = new();

        private void Start()
        {
            InitializeResources();
            resourcesManager.OnUpdateResources += UpdateResourceAmounts;
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void InitializeResources()
        {
            foreach (var resourcePair in resourcesManager.GetAllResources())
            {
                ResourceData resourceData = resourcePair.Value;

                if (resourceData.isUnlockedNow)
                {
                    GameObject newResourceUI = Instantiate(resourceUIPrefab, resourceUIContainer);
                    
                    _resourceUIElements[resourcePair.Key] = newResourceUI;
                    
                    Image resourceIcon = newResourceUI.GetComponentInChildren<Image>();
                    resourceIcon.sprite = resourceData.resourceConfig.resourceImage;
                    
                    TMP_Text resourceAmountText = newResourceUI.GetComponentInChildren<TMP_Text>();
                    resourceAmountText.text = resourceData.currentAmount.ToString();
                }
            }
        }

        private void UpdateResourceAmounts()
        {
            foreach (var resourcePair in resourcesManager.GetAllResources())
            {
                if (_resourceUIElements.ContainsKey(resourcePair.Key))
                {
                    ResourceData resourceData = resourcePair.Value;

                    TMP_Text resourceAmountText = _resourceUIElements[resourcePair.Key].GetComponentInChildren<TMP_Text>();
                    resourceAmountText.text = resourceData.currentAmount.ToString();
                }
            }
        }
        
        /*private void UpdateTextFields()
        {
            woodTextField.text = resourcesManager.GetResourceData(ResourceType.Wood).currentAmount.ToString();
            ironTextField.text = resourcesManager.GetResourceData(ResourceType.Iron).currentAmount.ToString();
            stoneTextField.text = resourcesManager.GetResourceData(ResourceType.Cotton).currentAmount.ToString();
            skinTextField.text = resourcesManager.GetResourceData(ResourceType.Skin).currentAmount.ToString();
        }*/
    }
}