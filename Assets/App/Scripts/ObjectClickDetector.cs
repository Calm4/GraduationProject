using UnityEngine;

namespace App.Scripts
{
    public class ObjectClickDetector : MonoBehaviour
    {
        private Camera _camera;

        private void Start()
        {
            _camera = Camera.main;
            if(!_camera)
                Debug.LogError("Can't find camera!");
        }

        void Update()
        {
            if (UnityEngine.Input.GetMouseButtonDown(0))
            {
                
                    var ray = _camera.ScreenPointToRay(UnityEngine.Input.mousePosition);

                    if (Physics.Raycast(ray, out var hit))
                    {
                        Debug.Log("Clicked on object: " + hit.collider.gameObject.name);
                    }
                
            }
        }
  

    }
}
