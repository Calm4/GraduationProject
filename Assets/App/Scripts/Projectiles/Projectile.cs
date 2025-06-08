using App.Scripts.Enemies;
using UnityEngine;

namespace App.Scripts.Projectiles
{
    public class Projectile : MonoBehaviour
    {
        private Enemy target;
        [SerializeField] private int damage;
        public float speed = 10f;
        
        public void Initialize(Enemy targetEnemy, int dmg)
        {
            target = targetEnemy;
            damage = dmg;
        }

        private void Update()
        {
            if (target == null)
            {
                Destroy(gameObject);
                return;
            }

            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, target.transform.position) < 0.1f)
            {
                target.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }
}