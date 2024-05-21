using System.Collections;
using System.Collections.Generic;
using CodeMonkey.Utils;
using UnityEngine;

public class Testing : MonoBehaviour
{
    private Grid _grid;

    void Start()
    {
        _grid = new Grid(5, 5, 10f);
        Debug.Log(_grid);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GetMousePosition();

        }

        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log(_grid.GetValue(GetMousePosition()));
        }
    }

    private Vector3 GetMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 mouseWorldPosition = hit.point;
            _grid.SetValue(mouseWorldPosition, 228);
        return mouseWorldPosition;
        }

        return Vector3.zero;
    }
}