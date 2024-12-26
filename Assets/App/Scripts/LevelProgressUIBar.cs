using System;
using System.Collections.Generic;
using App.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelProgressUIBar : MonoBehaviour
{
    [SerializeField] private ExperienceManager experienceManager;
    [Space(10)]
    [SerializeField] private Image progressBar;
    [SerializeField] private TMP_Text levelNumberField;
    
    
    private Dictionary<int,int> _levelProgressDictionary;

    private void Awake()
    {
        _levelProgressDictionary = experienceManager.LevelUpByExperienceDataBase.LevelProgressData;
        experienceManager.OnExperienceUpdate += UpdateUIProgressBar;
        experienceManager.OnLevelUp += UpdateUILevelNumber;

        progressBar.fillAmount = 0;
    }

    
    private void UpdateUIProgressBar(int currentLevelProgressValue)
    {
        if (!_levelProgressDictionary.
                TryGetValue(experienceManager.GetCurrentLevel(),out int requiredExperience)) return;
        
        progressBar.fillAmount = (float)currentLevelProgressValue / requiredExperience;
        Debug.Log(currentLevelProgressValue + "/" + requiredExperience);
    }
    
    private void UpdateUILevelNumber(int currentLevel)
    {
        levelNumberField.text = currentLevel.ToString();
    }
}
