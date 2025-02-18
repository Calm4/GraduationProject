using App.Scripts.Buildings;
using App.Scripts.Modifiers.Data;
using UnityEngine;

namespace App.Scripts.Modifiers
{
    [RequireComponent(typeof(LineRenderer))]
    public class BuildingRangeVisualizer : MonoBehaviour
    {
        private Building _visualizerOwner;
        private LineRenderer _lineRenderer;
        private float _radius;
        private int _segments = 50;
        private bool _isVisible;
        private bool _justShown = false; 
        private Vector2 _gridOffset = new Vector2(0.5f, 0.5f);

        private void Awake()
        {
            InitializeLineRenderer();
        }

        public void Initialize(Building building)
        {
            _visualizerOwner = building;
        }

        private void DrawCircle(float radius)
        {
            float angleStep = 360f / _segments;
            for (int i = 0; i <= _segments; i++)
            {
                float angle = Mathf.Deg2Rad * angleStep * i;
                Vector3 position = new Vector3(Mathf.Cos(angle) * radius, 0.01f, Mathf.Sin(angle) * radius);
                _lineRenderer.SetPosition(i, transform.position + new Vector3(_gridOffset.x,0,_gridOffset.y) + position);
            }
        }


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
            DrawCircle(range);
            _isVisible = true;
            _lineRenderer.enabled = true;
            _justShown = true;
        }

        private void HideVisualizer()
        {
            _isVisible = false;
            _lineRenderer.enabled = false;
        }

        private float GetRangeFromModifiers()
        {
            if (_visualizerOwner.ActiveModifiers.TryGetValue(ModifierType.Range, out ModifierInstance modifier))
            {
                if (modifier.ModifierData is RangeModifierData rangeData)
                {
                    return rangeData.currentRange;
                }
            }

            return 1f;
        }
        
        private void InitializeLineRenderer()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _lineRenderer.positionCount = _segments + 1;
            _lineRenderer.loop = true;
            _lineRenderer.startWidth = 0.1f;
            _lineRenderer.endWidth = 0.1f;
            _lineRenderer.enabled = false;
        }
    }
}