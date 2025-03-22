using System;
using App.Scripts.Buildings;
using App.Scripts.UI.Buttons;
using App.Scripts.UI.Windows;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace App.Scripts
{
    public class WindowOpener : MonoBehaviour
    {
        [Inject] private OpenPanelsManager _openPanelsManager;
        [Inject] private DiContainer _container;

        [SerializeField] private RectTransform windowToOpen;
        [SerializeField] private Button buttonToOpen;

        // Инжектируем родительский контейнер для UI-панелей
        [Inject(Id = "WindowsContainer")]
        private RectTransform _windowsContainer;        
        private Building _parentBuilding;

        public void Initialize(Building building)
        {
            _parentBuilding = building;
            Debug.Log("iNITIALIZED PARENT BUILDING:" + _parentBuilding);
        }

        private void OnEnable() => buttonToOpen.onClick.AddListener(OpenWindow);

        private void OnDisable() => buttonToOpen.onClick.RemoveListener(OpenWindow);

        private void Awake()
        {
            Debug.Log("DURAK TYPOY^ " + _parentBuilding);
        }

        private void OpenWindow()
        {
            GameObject instanceGo = _container.InstantiatePrefab(windowToOpen, _windowsContainer);

            RectTransform instance = instanceGo.GetComponent<RectTransform>();
            if (instance.TryGetComponent(out IBuildingButtonInitializer buildingButtonInitializer))
            {
                Debug.Log("2222 " + _parentBuilding);
                buildingButtonInitializer.BaseInitializer(_parentBuilding);
                _openPanelsManager.RegisterWindow(_parentBuilding, buildingButtonInitializer);
            }
            
            instance.anchoredPosition = Vector2.zero;
        }
    }
}