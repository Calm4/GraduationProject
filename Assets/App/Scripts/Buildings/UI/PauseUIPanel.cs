using System;
using App.Scripts.GameResources;
using App.Scripts.GameResources.Money;
using TMPro;
using UnityEngine;
using Zenject;

namespace App.Scripts.Buildings.UI
{
    public class PauseUIPanel : MonoBehaviour
    {
        [Inject] private ResourcesManager resourceManager;
        [SerializeField] private TMP_Text moneyTextField;

        private void Awake()
        {
            resourceManager.OnUpdateFinanceResources += UpdateFinanceResourceAmounts;
        }

        void Start()
        {
            moneyTextField.text = resourceManager.GetResourceData(ResourceType.Money).currentAmount.ToString();
        }

        private void UpdateFinanceResourceAmounts()
        {
            Debug.Log("FINANCES UPDATED");
            moneyTextField.text = resourceManager.GetResourceData(ResourceType.Money).currentAmount.ToString();
        }
    }
}