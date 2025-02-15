using UnityEngine;
using DG.Tweening;

namespace App.Scripts.Buildings.UI
{
    public abstract class UIClosableElement : MonoBehaviour
    {
        [SerializeField] protected float hideTime = 0.3f;
        [SerializeField] protected float clickDelayAfterSelection = 0.2f;

        private RectTransform _uiTransform;
        private bool _isShown = false;
        private float _lastBuildingClickTime = -1f;

        protected virtual void Awake()
        {
            _uiTransform = GetComponent<RectTransform>();
        }

        protected virtual void Update()
        {
            if (_isShown && UnityEngine.Input.GetMouseButtonDown(0))
            {
                if (Time.time - _lastBuildingClickTime < clickDelayAfterSelection)
                    return;

                if (!UIInteractionHelper.IsPointerOverUIElement(_uiTransform, UnityEngine.Input.mousePosition))
                {
                    Close();
                }
            }
        }

        protected virtual void Show()
        {
            _isShown = true;
        }

        protected virtual void Close()
        {
            _isShown = false;
        }
    }
}