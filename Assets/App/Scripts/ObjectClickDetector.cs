using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectClickDetector : MonoBehaviour
{
          void Update()
              {
                  if (Input.GetMouseButtonDown(0))
                  {
                      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                      RaycastHit hit;
          
                      if (Physics.Raycast(ray, out hit))
                      {
                          Debug.Log("Clicked on object: " + hit.collider.gameObject.name);
                      }
                  }
              }
  

}