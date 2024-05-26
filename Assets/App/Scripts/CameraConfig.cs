using UnityEngine;

namespace App.Scripts
{
    [CreateAssetMenu(fileName = "CameraConfig", menuName = "Configs/CameraConfig", order = 0)]
    public class CameraConfig : ScriptableObject
    {
        public float moveSpeed;
        public float zoomSpeed;
        public float rotationSpeed;
    }
}