using System;
using UnityEngine;
using App.Scripts.Enemies;
using UnityEngine.Serialization;

namespace App.Scripts.Buildings
{
    [RequireComponent(typeof(Collider))]
    public class CastleHealth : MonoBehaviour
    {
        public static event Action OnCastleDefeated;
        
        [FormerlySerializedAs("_currentHealth")]
        [Header("Castle Stats")]
        [SerializeField] private int currentHealth;
        [SerializeField] private int maxHealth = 100;

        private void Awake()
        {
            currentHealth = maxHealth;

            var meshCol = GetComponent<MeshCollider>();
            if (meshCol != null && !meshCol.convex)
            {
                Destroy(meshCol);
                var box = gameObject.AddComponent<BoxCollider>();
                box.isTrigger = true;
            }
            else
            {
                var col = GetComponent<Collider>();
                col.isTrigger = true;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            var enemy = other.GetComponent<Enemy>();
            if (enemy == null) return;

            int dmg = enemy.EnemyConfig.Damage;
            currentHealth -= dmg;
            Debug.Log($"Castle took {dmg} damage, HP now {currentHealth}");

            enemy.Die();

            if (currentHealth <= 0)
            {
                Debug.Log("You lost!");
                OnCastleDefeated?.Invoke(); // Вызываем событие
            }
        }
    
    }
}