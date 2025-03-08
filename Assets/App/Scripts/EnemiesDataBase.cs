using System.Collections.Generic;
using App.Scripts.Enemies;
using UnityEngine;

namespace App.Scripts
{
    [CreateAssetMenu(fileName = "EnemiesDataBase", menuName = "Configs/DataBases/EnemiesDataBase", order = 0)]
    public class EnemiesDataBase : ScriptableObject
    {
        public Enemy Cuban;
        public Enemy Spherik;
        public Enemy Piramidon;
        public Enemy Cylindron;
    }
}