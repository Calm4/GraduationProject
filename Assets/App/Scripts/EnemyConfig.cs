using UnityEngine;

namespace App.Scripts
{
    [CreateAssetMenu(fileName = "_EnemyConfig", menuName = "Configs/Gameplay Objects/Enemies", order = 0)]
    public class EnemyConfig : ScriptableObject
    {
        public int Health;
        public int Speed;
        public int ExperienceForKilling;
    }
}