using UnityEngine;

namespace App.Scripts
{
    public class EnemyClick : MonoBehaviour
    {
        [SerializeField] private ExperienceManager experienceManager;
        
        public void OnClick()
        {
            Destroy(gameObject);
            experienceManager.AddExperience(20);
        }
    }
}