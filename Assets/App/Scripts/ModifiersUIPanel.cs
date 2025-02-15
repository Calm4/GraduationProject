using System;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts
{
    public class ModifiersUIPanel : MonoBehaviour
    {
        [SerializeField] private Button closeWindowButton;

        private void Start()
        {
            closeWindowButton.onClick.AddListener(CloseWindow);
        }

        private void CloseWindow()
        {
            Destroy(gameObject);
        }
    }
}