using UnityEngine;

namespace App.Scripts
{
    [RequireComponent(typeof(LineRenderer))]
    public class RangeVisualizer : MonoBehaviour
    {
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

        public void ToggleVisibility()
        {
            _isVisible = !_isVisible;
            _lineRenderer.enabled = _isVisible;
        }
    }
}