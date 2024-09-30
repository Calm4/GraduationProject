using System.Collections.Generic;
using App.Scripts.GameResources.Money;
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
            resourcesManager.OnUpdateMaterialResources += UpdateResourceAmounts;
        }
        
        private void InitializeResources()
        {
            foreach (var resourcePair in resourcesManager.GetAllResources())
            {
                if (resourcePair.Value.resourceConfig is FinanceResourceConfig)
                {
                    continue;
                }

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
            Debug.Log("MATERIALS UPDATED");
            foreach (var resourcePair in resourcesManager.GetAllResources())
            {
                if (_resourceUIElements.ContainsKey(resourcePair.Key))
                {
                    ResourceData resourceData = resourcePair.Value;

                    TMP_Text resourceAmountText =
                        _resourceUIElements[resourcePair.Key].GetComponentInChildren<TMP_Text>();
                    resourceAmountText.text = resourceData.currentAmount.ToString();
                }
            }
        }
    }
}