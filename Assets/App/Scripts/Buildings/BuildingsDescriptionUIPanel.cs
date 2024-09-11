using System;
using System.Collections;
using System.Collections.Generic;
using App.Scripts.Buildings.BuildingsConfigs;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingsDescriptionUIPanel : MonoBehaviour
{

    [SerializeField] private BasicBuildingConfig basicBuildingConfig;

    [Title("Header Panel")] 
    [SerializeField] private Image buildingIcon;
    [SerializeField] private TMP_Text buildingName;
    [SerializeField] private TMP_Text buildingType;
    
    [Title("Description Panel")]
    [SerializeField] private TMP_Text descriptionTextField;

    [Title("Maintenance Panel")] 
    [SerializeField] private RectTransform maintenanceContainer;
    
    [Title("Construction Panel")]
    [SerializeField] private RectTransform constructionContainer;
    [SerializeField] private RectTransform resourcePrefab;

    [Title("Help Panel")] 
    [SerializeField] private TMP_Text helpTextField;
    
    public void Initialize(BasicBuildingConfig buildingConfig)
    {
        basicBuildingConfig = buildingConfig;
    }

    private void Start()
    {
        InitializeHeaderPanel();
        InitializeDescriptionPanel();
        InitializeMaintenancePanel();
        InitializeConstructionPanel();
        InitializeHelpPanel();
    }

    private void InitializeHeaderPanel()
    {
        buildingIcon.sprite = basicBuildingConfig.sprite;
        buildingName.text = basicBuildingConfig.buildingName;
        buildingType.text = basicBuildingConfig.buildingType.ToString();
    }

    private void InitializeDescriptionPanel()
    {
        descriptionTextField.text = basicBuildingConfig.buildingDescription;
    }
    
    private void InitializeMaintenancePanel()
    {
        
    }
    
    private void InitializeConstructionPanel()
    {
        for (int i = 0; i < basicBuildingConfig.resourcesToBuild.Count; i++)
        {
            var constructingResource = Instantiate(resourcePrefab, constructionContainer);
            //constructingResource.GetComponent<Image>().sprite = basicBuildingConfig.sprite/*.resourcesToBuild[i]*/;
            var resourceCount = constructingResource.GetComponentInChildren<TMP_Text>();
            resourceCount.text = basicBuildingConfig.resourcesToBuild[i].amountToBuild.ToString();
        }
    }

    private void InitializeHelpPanel()
    {
        
    }
}
