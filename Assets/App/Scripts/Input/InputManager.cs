using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace App.Scripts.Input
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private UnityEngine.Camera sceneCamera;
        [SerializeField] private LayerMask placementLayerMask;
        private Vector3 _lastPosition;
    
        public event Action OnClicked;
        public event Action OnExit;

        public event Action OnKeyboardInput; 
        public event Action OnScroll;
        public event Action OnRotateAround;

        private void Update()
        {
            if (UnityEngine.Input.GetMouseButtonDown(0))
            {
                OnClicked?.Invoke();
            }

            if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
            {
                OnExit?.Invoke();
            }

            if (UnityEngine.Input.GetMouseButton(2))
            {
                OnRotateAround?.Invoke();
            }

            if (UnityEngine.Input.GetAxis("Mouse ScrollWheel") != 0f)
            {
                OnScroll?.Invoke();
            }
        
            OnKeyboardInput?.Invoke();
        }


        public bool IsPointerOverUI() => EventSystem.current.IsPointerOverGameObject();
    
        public Vector3 GetSelectedMapPosition()
        {
            Vector3 mousePos = UnityEngine.Input.mousePosition;
            mousePos.z = sceneCamera.nearClipPlane;
            Ray ray = sceneCamera.ScreenPointToRay(mousePos);
            
            if (Physics.Raycast(ray, out var hit, 100, placementLayerMask))
            {
                _lastPosition = hit.point;
            }

            return _lastPosition;
        }

        public Vector2 GetMovementDirection()
        {
            float horizontal = UnityEngine.Input.GetAxisRaw("Horizontal");
            float vertical = UnityEngine.Input.GetAxisRaw("Vertical");

            return new Vector2(horizontal, vertical);
        }
    }
}
