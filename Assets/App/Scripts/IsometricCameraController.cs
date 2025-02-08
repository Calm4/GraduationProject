using Cinemachine;
using UnityEngine;

namespace App.Scripts
{
    [RequireComponent(typeof(Transform))]
    public class IsometricCameraController : MonoBehaviour
    {
        #region Settings

        [Header("Cinemachine Settings")]
        [SerializeField] private CinemachineVirtualCamera virtualCamera;

        [Header("Movement Settings")]
        [SerializeField] private float mousePanMultiplier = 0.5f;   
        [SerializeField] private float moveSmoothTime = 0.1f;        

        [Header("Zoom Settings")]
        [SerializeField] private float zoomSpeed = 5f;               
        [SerializeField] private float minOrthographicSize = 2f;     
        [SerializeField] private float maxOrthographicSize = 10f;    
        [SerializeField] private float pinchZoomSensitivity = 0.01f; 

        #endregion

        private Vector3 _moveVelocity = Vector3.zero;
        private Vector3 _targetPosition;

        void Start()
        {
            _targetPosition = transform.position;
        }

        void Update()
        {
            ProcessInput();
            ProcessZoom();

            transform.position = Vector3.SmoothDamp(transform.position, _targetPosition, ref _moveVelocity, moveSmoothTime);
        }
        
        private void ProcessInput()
        {
            //только для ПКМ (правой кнопки мыши)
            if (UnityEngine.Input.GetMouseButton(1))
            {
                float mouseX = UnityEngine.Input.GetAxis("Mouse X");
                float mouseY = UnityEngine.Input.GetAxis("Mouse Y");

                Vector3 camForward = virtualCamera.transform.forward;
                camForward.y = 0f;
                camForward.Normalize();

                Vector3 camRight = virtualCamera.transform.right;
                camRight.y = 0f;
                camRight.Normalize();

                Vector3 movement = (camRight * -mouseX + camForward * -mouseY) * mousePanMultiplier;
                _targetPosition += movement;
            }
        }

        /// <summary>
        /// - На ПК – колесико мыши.
        /// - На мобильном – pinch-gesture с двумя пальцами.
        /// </summary>
        private void ProcessZoom()
        {
            // android
            if (UnityEngine.Input.touchCount == 2)
            {
                Touch touch0 = UnityEngine.Input.GetTouch(0);
                Touch touch1 = UnityEngine.Input.GetTouch(1);

                if (touch0.phase == TouchPhase.Moved || touch1.phase == TouchPhase.Moved)
                {
                    float prevTouchDeltaMag = ((touch0.position - touch0.deltaPosition) - (touch1.position - touch1.deltaPosition)).magnitude;
                    float touchDeltaMag = (touch0.position - touch1.position).magnitude;
                    float deltaMagnitudeDiff = touchDeltaMag - prevTouchDeltaMag;

                    float newSize = virtualCamera.m_Lens.OrthographicSize - deltaMagnitudeDiff * zoomSpeed * pinchZoomSensitivity;
                    virtualCamera.m_Lens.OrthographicSize = Mathf.Clamp(newSize, minOrthographicSize, maxOrthographicSize);
                }
            }
            else // ПК
            {
                float scroll = UnityEngine.Input.GetAxis("Mouse ScrollWheel");
                if (Mathf.Abs(scroll) > Mathf.Epsilon)
                {
                    float newSize = virtualCamera.m_Lens.OrthographicSize - scroll * zoomSpeed;
                    virtualCamera.m_Lens.OrthographicSize = Mathf.Clamp(newSize, minOrthographicSize, maxOrthographicSize);
                }
            }
        }
    }
}
