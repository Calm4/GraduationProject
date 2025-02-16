using UnityEngine;
using Zenject;

namespace App.Scripts.Enemies
{
    public class EnemyClick : MonoBehaviour
    {
        [Inject] private ExperienceManager _experienceManager;
        
        public void OnClick()
        {
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            //TODO: сделать добавление экспы при убийстве, а не при уничтожении
            _experienceManager.AddExperience(20);
        }
    }
}