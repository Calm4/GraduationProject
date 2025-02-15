using App.Scripts.Buildings;
using App.Scripts.Modifiers;
using App.Scripts.Modifiers.Data;
using UnityEngine;

namespace App.Scripts
{
    [RequireComponent(typeof(LineRenderer))]
    public class RangeVisualizer : MonoBehaviour
    {
        private Building _buildingOwner;
        private LineRenderer _lineRenderer;
        private float _radius;
        private int _segments = 50;
        private bool _isVisible = false;

        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _lineRenderer.positionCount = _segments + 1;
            _lineRenderer.loop = true;
            _lineRenderer.startWidth = 0.1f;
            _lineRenderer.endWidth = 0.1f;
            _lineRenderer.enabled = false; // Изначально отключаем
        }

        public void Initialize(Building buildingOwner)
        {
            _buildingOwner = buildingOwner;
        }

        public void SetRadius(float radius)
        {
            _radius = radius;
            DrawCircle();
        }

        private void DrawCircle()
        {
            float angleStep = 360f / _segments;
            for (int i = 0; i <= _segments; i++)
            {
                float angle = Mathf.Deg2Rad * angleStep * i;
                Vector3 position = new Vector3(Mathf.Cos(angle) * _radius, 0.01f, Mathf.Sin(angle) * _radius);
                _lineRenderer.SetPosition(i, transform.position + position);
            }
        }
        
        private bool _justShown = false;

        private void Update()
        {
            if (_justShown) 
            {
                _justShown = false;
                return;
            }

            if (_isVisible && UnityEngine.Input.GetMouseButtonDown(0))
            {
                if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
                {
                    HideVisualizer();
                }
            }
        }

        public void ShowVisualizer()
        {
            float range = GetRangeFromModifiers();
            SetRadius(range);
            Show();
            _justShown = true;
        }


        private void Show()
        {
            Debug.Log("RangeVisualizer: Show");
            _isVisible = true;
            _lineRenderer.enabled = true;
        }

        public void HideVisualizer()
        {
            Debug.Log("RangeVisualizer: Hide");
            _isVisible = false;
            _lineRenderer.enabled = false;
        }


        
        public float GetRangeFromModifiers()
        {
            if (_buildingOwner.ActiveModifiers.TryGetValue(ModifierType.Range, out ModifierInstance modifier))
            {
                if (modifier.ModifierData is RangeModifierData rangeData)
                {
                    return rangeData.currentRange;
                }
            }
            return 1f;
        }
        
        public void ToggleVisibility()
        {
            _isVisible = !_isVisible;
            _lineRenderer.enabled = _isVisible;
        }
    }
}
