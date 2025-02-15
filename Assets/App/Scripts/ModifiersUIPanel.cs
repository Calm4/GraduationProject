using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts
{
    public class ModifiersUIPanel : MonoBehaviour
    {
        [SerializeField] private Button closeWindowButton;
        [SerializeField] private RectTransform headerPanel;

        private bool isDragging = false;
        private Vector2 offset;

        private void Start()
        {
            closeWindowButton.onClick.AddListener(CloseWindow);
        }

        private void Update()
        {
            if (isDragging)
            {
                Vector2 mousePosition = UnityEngine.Input.mousePosition;
                RectTransform panelRect = GetComponent<RectTransform>();
                panelRect.position = mousePosition - offset;
            }

            if (UnityEngine.Input.GetMouseButtonDown(0)) // ЛКМ
            {
                if (RectTransformUtility.RectangleContainsScreenPoint(headerPanel, UnityEngine.Input.mousePosition))
                {
                    isDragging = true;
                    RectTransform panelRect = GetComponent<RectTransform>();
                    offset = UnityEngine.Input.mousePosition - panelRect.position;
                }
            }

            if (UnityEngine.Input.GetMouseButtonUp(0)) // Отпустили ЛКМ
            {
                isDragging = false;
            }
        }

        private void CloseWindow()
        {
            Destroy(gameObject);
        }
    }
}