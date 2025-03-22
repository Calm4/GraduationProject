using UnityEngine;

namespace App.Scripts.Input
{
    public class ObjectClickDetector : MonoBehaviour
    {
        private UnityEngine.Camera _camera;

        private void Start()
        {
            _camera = UnityEngine.Camera.main;
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
                        //Debug.Log("Clicked on object: " + hit.collider.gameObject.name);
                    }
                
            }
        }
  

    }
}
