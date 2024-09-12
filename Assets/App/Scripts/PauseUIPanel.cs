using System.Collections;
using System.Collections.Generic;
using System.Resources;
using App.Scripts.Resources;
using TMPro;
using UnityEngine;

public class PauseUIPanel : MonoBehaviour
{
    [SerializeField] private ResourcesManager resourceManager;
    [SerializeField] private TMP_Text moneyTextField;
    
    void Start()
    {
        moneyTextField.text = resourceManager.GetResourceData(ResourceType.Money).currentAmount.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
