using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Camera sceneCamera;
    [SerializeField] private LayerMask placementLayerMask;
    private Vector3 _lastPosition;
    
    public event Action OnClicked;
    public event Action OnExit;

    public event Action OnKeyboardInput; 
    public event Action OnScroll;
    public event Action OnRotateAround;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnClicked?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnExit?.Invoke();
        }

        if (Input.GetMouseButton(2))
        {
            OnRotateAround?.Invoke();
        }

        if (Input.GetAxis("Mouse ScrollWheel") != 0f)
        {
            OnScroll?.Invoke();
        }
        
        OnKeyboardInput?.Invoke();
    }


    public bool IsPointerOverUI() => EventSystem.current.IsPointerOverGameObject();
    
    public Vector3 GetSelectedMapPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = sceneCamera.nearClipPlane;
        Ray ray = sceneCamera.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, placementLayerMask))
        {
            _lastPosition = hit.point;
        }

        return _lastPosition;
    }

    public Vector2 GetMovementDirection()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        return new Vector2(horizontal, vertical);
    }
}
