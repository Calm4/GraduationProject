using System;
using System.Collections.Generic;
using App.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class LevelProgressUIBar : MonoBehaviour
{
    [Inject] private ExperienceManager _experienceManager;
    [Space(10)]
    [SerializeField] private Image progressBar;
    [SerializeField] private TMP_Text levelNumberField;
    
    private void Awake()
    {
        _experienceManager.OnExperienceUpdate += UpdateUIProgressBar;
        _experienceManager.OnLevelUp += UpdateUILevelNumber;

        progressBar.fillAmount = 0;
    }
    
    private void UpdateUIProgressBar(int currentLevelProgressValue)
    {
        if (!_experienceManager.GetExperienceDictionary().
                TryGetValue(_experienceManager.GetCurrentLevel(),out int requiredExperience)) return;
        
        progressBar.fillAmount = (float)currentLevelProgressValue / requiredExperience;
        Debug.Log(currentLevelProgressValue + "/" + requiredExperience);
    }
    
    private void UpdateUILevelNumber(int currentLevel)
    {
        levelNumberField.text = currentLevel.ToString();
    }
}
