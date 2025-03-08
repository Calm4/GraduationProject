using System.Collections;
using App.Scripts.Buildings;
using UnityEngine;

public class UIFollowCamera : MonoBehaviour
{
    private Camera _mainCamera;
    private Transform _buildingTransform;
    private CanvasGroup _canvasGroup;

    [Header("Настройки")]
    public float minScaleDistance = 10f;
    public float maxScaleDistance = 50f;
    public float minScale = 0.5f;
    public float maxScale = 1f;

    private bool _isPanelVisible = false;

    private void Start()
    {
        _mainCamera = Camera.main;
        _buildingTransform = transform.parent;
        _canvasGroup = GetComponent<CanvasGroup>();

        // Сначала панель скрыта
        _canvasGroup.alpha = 0;

        // Подписываемся на событие клика по зданию
        Building.OnBuildingClicked += HandleBuildingClicked;
    }

    private void Update()
    {
        // Если панель видна, продолжаем её следить за объектом
        if (_isPanelVisible)
        {
            // Фиксируем панель по вертикали (без вращения)
            Vector3 fixedPosition = _buildingTransform.position + Vector3.up * 3f;
            transform.position = fixedPosition;

            // Панель не вращается
            transform.rotation = Quaternion.Euler(90, 0, -45);

            // Масштаб панели в зависимости от расстояния
            float distance = Vector3.Distance(_mainCamera.transform.position, _buildingTransform.position);
            float scale = Mathf.Lerp(maxScale, minScale, Mathf.InverseLerp(minScaleDistance, maxScaleDistance, distance));
            transform.localScale = Vector3.one * scale;
        }
    }

    private void HandleBuildingClicked(Building building)
    {
        // Показать панель при клике на здание
        _isPanelVisible = true;
        _canvasGroup.alpha = 1;
    }

    // Метод для скрытия панели, если нужно
    public void HidePanel()
    {
        _isPanelVisible = false;
        _canvasGroup.alpha = 0;
    }

    private void OnDestroy()
    {
        // Отписываемся от события, когда объект уничтожается
        Building.OnBuildingClicked -= HandleBuildingClicked;
    }
}
