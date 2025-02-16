using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace App.Scripts
{
    public class DraggingPanel : MonoBehaviour
    {
       [SerializeField] private RectTransform fullPanel;
       private RectTransform _panelHeader;

        private bool _isDragging = false;
        private Vector2 _offset;

        private void Awake()
        {
            _panelHeader = GetComponent<RectTransform>();
        }


        private void Update()
        {
            DragPanel();
        }

        public void DragPanel()
        {
            if (_isDragging)
            {
                Vector2 mousePosition = UnityEngine.Input.mousePosition;
                // Вычисляем новую позицию с учетом смещения
                Vector2 newPos = mousePosition - _offset;

                // Получаем размеры панели (в локальных единицах)
                Vector2 panelSize = fullPanel.rect.size;
                // Если pivot = (0.5, 0.5), то можно использовать половину размера
                Vector2 halfSize = panelSize * 0.5f;

                // Ограничиваем позицию по оси X и Y
                newPos.x = Mathf.Clamp(newPos.x, halfSize.x, Screen.width - halfSize.x);
                newPos.y = Mathf.Clamp(newPos.y, halfSize.y, Screen.height - halfSize.y);

                fullPanel.position = newPos;
            }

            if (UnityEngine.Input.GetMouseButtonDown(0)) // ЛКМ нажата
            {
                if (RectTransformUtility.RectangleContainsScreenPoint(_panelHeader, UnityEngine.Input.mousePosition))
                {
                    _isDragging = true;
                    _offset = UnityEngine.Input.mousePosition - fullPanel.position;
                }
            }

            if (UnityEngine.Input.GetMouseButtonUp(0)) // ЛКМ отпущена
            {
                _isDragging = false;
            }
        }

    }
}