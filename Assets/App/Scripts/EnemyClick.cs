using UnityEngine;
using Zenject;

namespace App.Scripts
{
    public class EnemyClick : MonoBehaviour
    {
        [Inject] private ExperienceManager _experienceManager;
        
        public void OnClick()
        {
            Destroy(gameObject);
            _experienceManager.AddExperience(20);
        }
    }
}