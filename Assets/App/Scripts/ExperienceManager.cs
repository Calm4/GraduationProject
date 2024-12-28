using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace App.Scripts
{
    public class ExperienceManager : MonoBehaviour
    {
        [SerializeField] private int currentExperience;
        [SerializeField] private LevelUpProgressDataBase levelUpByExperienceDataBase;
        
        private int _currentLevel = 1;

        public event Action<int> OnExperienceUpdate;
        public event Action<int> OnLevelUp;

        public void AddExperience(int experienceToAdd)
        {
            currentExperience += experienceToAdd;
            CheckExperienceToLevelUp();
            OnExperienceUpdate?.Invoke(currentExperience);
        }

        public void ReduceExperience(int experienceToReduce)
        {
            currentExperience -= experienceToReduce;
        }
        public void ResetExperience()
        {
            currentExperience = 0;
        }

        private void LevelUp()
        {
            _currentLevel++;
            OnLevelUp?.Invoke(_currentLevel); 
            currentExperience = 0;
        }
        
        private void CheckExperienceToLevelUp()
        {
            if (!levelUpByExperienceDataBase.LevelProgressData
                    .TryGetValue(_currentLevel, out int requiredExperience)) return;
            
            if (currentExperience >= requiredExperience)
            {
                LevelUp();
            }
        }

        public int GetCurrentLevel()
        {
            return _currentLevel;
        }

        public Dictionary<int, int> GetExperienceDictionary()
        {
            return levelUpByExperienceDataBase.LevelProgressData;
        }
    }
}