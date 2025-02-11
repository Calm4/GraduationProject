using App.Scripts.Enemies;
using UnityEngine;

namespace App.Scripts
{
    public class Projectile : MonoBehaviour
    {
        private Enemy target;
        private int damage;
        public float speed = 10f;

        /// <summary>
        /// Инициализирует пулю с целью и уроном.
        /// </summary>
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

            // Перемещаем пулю к цели
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);

            // Если пуля достигла цели, наносим урон и уничтожаем пулю
            if (Vector3.Distance(transform.position, target.transform.position) < 0.1f)
            {
                target.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }
}