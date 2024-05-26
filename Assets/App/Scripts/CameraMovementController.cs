using System;
using System.Collections;
using System.Collections.Generic;
using App.Scripts;
using UnityEngine;

public class CameraMovementController : MonoBehaviour
{
    [SerializeField] private CameraConfig cameraConfig;
    [SerializeField] private InputManager inputManager;

    private void Start()
    {
        inputManager.OnKeyboardInput += CameraMovement;
        inputManager.OnScroll += CameraZoom;
        inputManager.OnRotateAround += CameraRotation;
    }

    private void CameraRotation()
    {
        float rotation = Input.GetAxis("Mouse X");

        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);

        if (Physics.Raycast(ray, out hit, 100))
        {
            Debug.Log("Hitted");
            transform.RotateAround(hit.point, Vector3.up, rotation * cameraConfig.rotationSpeed * Time.deltaTime);
        }
    }

    private void CameraZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        Vector3 zoom = transform.forward * (scroll * cameraConfig.zoomSpeed);
        transform.Translate(zoom, Space.World);
    }

    private void CameraMovement()
    {
        Vector3 horizontalMovement = transform.right * inputManager.GetMovementDirection().x;
        Vector3 forwardOnXZ = Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;
        Vector3 verticalMovement = forwardOnXZ * inputManager.GetMovementDirection().y;


        Vector3 moveDirection = horizontalMovement + verticalMovement;

        if (moveDirection.sqrMagnitude > 1)
        {
            moveDirection.Normalize();
        }

        transform.Translate(moveDirection * (cameraConfig.moveSpeed * Time.deltaTime), Space.World);
    }
}