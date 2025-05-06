using UnityEngine;
using TMPro;
using DG.Tweening;
using App.Scripts.GameResources;
using UnityEngine.UI;

namespace App.Scripts.GameResources
{
    public class ResourceDropVisual : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI amountText;
        [SerializeField] private Image resourceIcon;
        [SerializeField] private float floatHeight = 1f;
        [SerializeField] private float floatDuration = 1f;
        [SerializeField] private float fadeDuration = 0.5f;
        
        private void Awake()
        {
            // Настраиваем Canvas для отображения поверх всего
            var canvas = GetComponent<Canvas>();
            if (canvas != null)
            {
                canvas.sortingLayerName = "UI";
                canvas.sortingOrder = 32767; // Максимальное значение для sorting order
            }
        }
        
        private void Start()
        {
            // Убедимся, что все элементы видимы
            amountText.alpha = 1f;
            resourceIcon.color = new Color(1f, 1f, 1f, 1f);
            
            // Анимация подъема вверх
            transform.DOMoveY(transform.position.y + floatHeight, floatDuration)
                .SetEase(Ease.OutQuad);
            
            // Анимация затухания
            amountText.DOFade(0f, fadeDuration)
                .SetDelay(floatDuration - fadeDuration);
            resourceIcon.DOFade(0f, fadeDuration)
                .SetDelay(floatDuration - fadeDuration)
                .OnComplete(() => Destroy(gameObject));
        }

        private void LateUpdate()
        {
            // Поворачиваем текст к камере
            if (Camera.main != null)
            {
                transform.rotation = Camera.main.transform.rotation;
            }
        }

        public void Initialize(ResourceType resourceType, int amount, Sprite icon)
        {
            amountText.text = $"X{amount}";
            if (icon != null)
            {
                resourceIcon.sprite = icon;
            }
        }
    }
} 