using UnityEngine;

namespace App.Scripts
{
    public class ObjectClickDetector : MonoBehaviour
    {
        void Update()
        {
            if (UnityEngine.Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition);
                RaycastHit hit;
          
                if (Physics.Raycast(ray, out hit))
                {
                    Debug.Log("Clicked on object: " + hit.collider.gameObject.name);
                }
            }
        }
  

    }
}
