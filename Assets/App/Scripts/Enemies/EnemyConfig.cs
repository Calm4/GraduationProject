using System;
using System.Collections.Generic;
using App.Scripts.GameResources;
using UnityEngine;

namespace App.Scripts.Enemies
{
    [CreateAssetMenu(fileName = "_EnemyConfig", menuName = "Configs/Gameplay Objects/Enemies", order = 0)]
    public class EnemyConfig : ScriptableObject
    {
        public int Health;
        public int Speed;
        public int ExperienceForKilling;
        public int Damage;
        
        [Serializable]
        public struct DroppableResource
        {
            public ResourceType resourceType;
            public int minAmount;
            public int maxAmount;
            [Range(0f, 1f)] public float dropChance;
        }
        
        public List<DroppableResource> droppableResources;
    }
}