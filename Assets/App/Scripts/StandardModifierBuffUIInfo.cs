using App.Scripts.Modifiers;
using App.Scripts.Modifiers.Configs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts
{
    public class StandardModifierBuffUIInfo : MonoBehaviour
    {
        [SerializeField] private Image buffImage; 
        [SerializeField] private TMP_Text buffText;
        [SerializeField] private ModifierType modifierType;
        private BaseModifierSO _baseModifierSO;

        public void Initialize(StandardModifiersSO baseModifierSO)
        {
            _baseModifierSO = baseModifierSO;
            InitializePanel();
        }

        private void InitializePanel()
        {
            buffImage.sprite = _baseModifierSO.modifierIcon;
            buffText.text = _baseModifierSO.modifierName;
        }
    }
}