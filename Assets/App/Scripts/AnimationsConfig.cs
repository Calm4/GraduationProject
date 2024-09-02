using UnityEngine;

namespace App.Scripts
{
    [CreateAssetMenu(fileName = "AnimationsConfig", menuName = "Configs/AnimationsConfig", order = 0)]
    public class AnimationsConfig : ScriptableObject
    {
        public float panelHideTime;
        public float panelShowTime;
        public float panelImageRotateTime;
        public float buildingPlacingTime;
    }
}