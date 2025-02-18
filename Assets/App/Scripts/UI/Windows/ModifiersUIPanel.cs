using App.Scripts.Buildings;
using App.Scripts.Modifiers;
using App.Scripts.UI.Buttons;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace App.Scripts.UI.Windows
{
    public class ModifiersUIPanel : MonoBehaviour, IBuildingButtonInitializer
    {
        [Inject] private OpenPanelsManager _openPanelsManager;
        
        [SerializeField] private Button closeWindowButton; 
        [SerializeField] private RectTransform panelHeader;
        [SerializeField] private RectTransform panelBody;
        [SerializeField] private ModifierRowPanel modifierRowPanel;

        private Building _parentBuilding;
        

        public void BaseInitializer(Building parentBuilding)
        {
            _parentBuilding = parentBuilding;
            _parentBuilding.ModifierManager.OnModifierAdded += OnModifierAdded;
            FillModifiersPanel();
        }

        private void OnModifierAdded(ModifierInstance modifier)
        {
            InitializeModifierRow(modifier);
        }


        private void Start()
        {
            closeWindowButton.onClick.AddListener(CloseWindow);
        }

        private void FillModifiersPanel()
        {
            if (_parentBuilding == null)
            {
                Debug.LogError("Parent building is not set.");
                return;
            }

            foreach (Transform child in panelBody)
            {
                Destroy(child.gameObject);
            }

            foreach (var modifier in _parentBuilding.ActiveModifiers.Values)
            {
                InitializeModifierRow(modifier);
            }
        }


        private void InitializeModifierRow(ModifierInstance modifier)
        {
            var modifierInstance = Instantiate(modifierRowPanel, panelBody);
            modifierInstance.MainImage.sprite = modifier.ModifierData.Config.modifierIcon;
            modifierInstance.ModifierText.text = modifier.ModifierData.Config.modifierName;

        }
        
        private void CloseWindow()
        {
            if (_parentBuilding != null)
            {
                _openPanelsManager.UnregisterWindow(_parentBuilding);
            }
            Destroy(gameObject);
        }
    }
}