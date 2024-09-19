using System.Collections.Generic;
using App.Scripts.Buildings.BuildingsConfigs;
using App.Scripts.GameResources;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.Buildings
{
    public class BuildingsDescriptionUIPanel : MonoBehaviour
    {
        [SerializeField] private RectTransform mainPanel;
        [SerializeField] private RectTransform resourcePrefab;
        [SerializeField] private BasicBuildingConfig basicBuildingConfig;
        
        [Title("Header Panel")] 
        [SerializeField] private Image buildingIcon;
        [SerializeField] private TMP_Text buildingName;
        [SerializeField] private TMP_Text buildingType;
    
        [Title("Description Panel")]
        [SerializeField] private TMP_Text descriptionTextField;

        [Title("Maintenance Panel")] 
        [SerializeField] private RectTransform maintenanceContainer;
        [SerializeField] private Color maintenanceColor;
    
        [Title("Construction Panel")]
        [SerializeField] private RectTransform constructionContainer;
        [SerializeField] private Color constructionColor;

        [Title("Incoming Panel")] 
        [SerializeField] private RectTransform incomingContainer;
        [SerializeField] private Color incomingColor;
        
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
            InitializeIncomingPanel();
            InitializeHelpPanel();

            DisableInitializedAllPanels();
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
            InstantiateResource(basicBuildingConfig.maintenanceResources, maintenanceContainer, maintenanceColor);
        }
    
        private void InitializeConstructionPanel()
        {
            InstantiateResource(basicBuildingConfig.resourcesToBuild, constructionContainer, constructionColor);
        }
        private void InitializeIncomingPanel()
        {
            InstantiateResource(basicBuildingConfig.incomingResources, incomingContainer, incomingColor);
        }

        private void InstantiateResource(List<ResourceRequirement> resources, RectTransform container, Color color)
        {
            for (int i = 0; i < resources.Count; i++)
            {
                var constructingResource = Instantiate(resourcePrefab, container);
            
                var resourceImage = constructingResource.GetComponentInChildren<Image>();
                resourceImage.sprite = resources[i].config.resourceImage;
                
                var resourceAmountToBuild = constructingResource.GetComponentInChildren<TMP_Text>();
                resourceAmountToBuild.text = resources[i].amount.ToString();
                resourceAmountToBuild.color = color;
            }
        }

        private void InitializeHelpPanel()
        {
        
        }
        private void DisableInitializedAllPanels()
        {
            mainPanel.gameObject.SetActive(false);
        }
    }
}
