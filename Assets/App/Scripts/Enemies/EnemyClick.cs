using App.Scripts.Experience;
using UnityEngine;
using Zenject;

namespace App.Scripts.Enemies {
    public class EnemyClick : MonoBehaviour {
        [Inject] private ExperienceManager _experienceManager;

        public void OnClick() {
            var enemy = GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.Die();
                _experienceManager.AddExperience(enemy.EnemyConfig.ExperienceForKilling);
            }
        }
    }
}