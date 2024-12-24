using System;
using System.Collections.Generic;
using App.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelProgressUIBar : MonoBehaviour
{
    [SerializeField] private Image progressBar;
    [SerializeField] private TMP_Text levelNumberField;
    [SerializeField] private LevelUpProgressDataBase levelUpProgressData;
    
    public event Action OnExperienceGained;
    
    private Dictionary<int,int> _levelProgressDictionary;

    private void Awake()
    {
        _levelProgressDictionary = levelUpProgressData.LevelProgressData;
    }
    
}
