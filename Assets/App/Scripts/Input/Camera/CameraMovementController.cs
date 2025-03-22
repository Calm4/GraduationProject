using UnityEngine;
using Zenject;

namespace App.Scripts.Input.Camera
{
    public class CameraMovementController : MonoBehaviour
    {
        [SerializeField] private CameraConfig cameraConfig;
        [Inject] private InputManager _inputManager;

        private void Start()
        {
            _inputManager.OnKeyboardInput += CameraMovement;
            _inputManager.OnScroll += CameraZoom;
            _inputManager.OnRotateAround += CameraRotation;
        }

        private void CameraRotation()
        {
            float rotation = UnityEngine.Input.GetAxis("Mouse X");

            Ray ray = new Ray(transform.position, transform.forward);

            Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);

            if (Physics.Raycast(ray, out var hit, 100))
            {
                Debug.Log("Hit");
                transform.RotateAround(hit.point, Vector3.up, rotation * cameraConfig.rotationSpeed * Time.deltaTime);
            }
        }

        private void CameraZoom()
        {
            float scroll = UnityEngine.Input.GetAxis("Mouse ScrollWheel");
            Vector3 zoom = transform.forward * (scroll * cameraConfig.zoomSpeed);
            transform.Translate(zoom, Space.World);
        }

        private void CameraMovement()
        {
            Vector3 horizontalMovement = transform.right * _inputManager.GetMovementDirection().x;
            Vector3 forwardOnXZ = Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;
            Vector3 verticalMovement = forwardOnXZ * _inputManager.GetMovementDirection().y;


            Vector3 moveDirection = horizontalMovement + verticalMovement;

            if (moveDirection.sqrMagnitude > 1)
            {
                moveDirection.Normalize();
            }

            transform.Translate(moveDirection * (cameraConfig.moveSpeed * Time.deltaTime), Space.World);
        }
    }
}