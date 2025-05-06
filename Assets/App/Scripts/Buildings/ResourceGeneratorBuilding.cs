using UnityEngine;
using App.Scripts.GameResources;
using App.Scripts.TurnsBasedSystem;
using App.Scripts.Buildings.BuildingsConfigs;
using Zenject;

namespace App.Scripts.Buildings
{
    public class ResourceGeneratorBuilding : MonoBehaviour
    {
        private ResourcesManager _resourcesManager;
        private GamePhaseManager _gamePhaseManager;
        private ResourceDropVisualFactory _visualFactory;
        
        private BasicBuildingConfig _buildingConfig;
        private bool _isInitialized;
        private Building _building;
        private GamePhase _lastProcessedPhase;
        
        private void Awake()
        {
            Debug.Log($"ResourceGeneratorBuilding.Awake called on {gameObject.name}");
            _building = GetComponent<Building>();
            if (_building == null)
            {
                Debug.LogError($"Building component not found on {gameObject.name}");
                return;
            }

            _gamePhaseManager = FindObjectOfType<GamePhaseManager>();
            if (_gamePhaseManager == null)
            {
                Debug.LogError("GamePhaseManager not found in scene!");
                return;
            }

            _resourcesManager = FindObjectOfType<ResourcesManager>();
            if (_resourcesManager == null)
            {
                Debug.LogError("ResourcesManager not found in scene!");
                return;
            }

            _visualFactory = FindObjectOfType<ResourceDropVisualFactory>();
            if (_visualFactory == null)
            {
                Debug.LogWarning("ResourceDropVisualFactory not found in scene, visual effects will be disabled");
            }
        }

        private void Start()
        {
            Debug.Log($"ResourceGeneratorBuilding.Start called on {gameObject.name}");
            Initialize();
        }

        private void Initialize()
        {
            if (_isInitialized)
            {
                Debug.Log($"ResourceGeneratorBuilding already initialized on {gameObject.name}");
                return;
            }

            if (_building == null)
            {
                Debug.LogError($"Building component is null on {gameObject.name}");
                return;
            }

            _buildingConfig = _building.BuildingConfig;
            if (_buildingConfig == null)
            {
                Debug.LogError($"BuildingConfig is null on {gameObject.name}");
                return;
            }

            if (_gamePhaseManager == null)
            {
                Debug.LogError($"GamePhaseManager is null on {gameObject.name}");
                return;
            }

            if (_resourcesManager == null)
            {
                Debug.LogError($"ResourcesManager is null on {gameObject.name}");
                return;
            }

            _gamePhaseManager.OnGameStateChanges += OnGameStateChanged;
            _isInitialized = true;
            _lastProcessedPhase = _gamePhaseManager.GetCurrentGameState();
            
            Debug.Log($"ResourceGeneratorBuilding initialized for {_buildingConfig.buildingName} with {_buildingConfig.incomingResources?.Count ?? 0} incoming resources");
        }

        private void OnDestroy()
        {
            if (_gamePhaseManager != null)
            {
                _gamePhaseManager.OnGameStateChanges -= OnGameStateChanged;
            }
        }

        private void OnGameStateChanged(GamePhase newPhase)
        {
            if (!_isInitialized)
            {
                Debug.LogWarning($"ResourceGeneratorBuilding not initialized on {gameObject.name}, trying to initialize...");
                Initialize();
                if (!_isInitialized) return;
            }

            // Проверяем, не обрабатывали ли мы уже эту фазу
            if (newPhase == _lastProcessedPhase)
            {
                Debug.Log($"Phase {newPhase} already processed for {_buildingConfig.buildingName}, skipping");
                return;
            }
            
            Debug.Log($"Phase changed to {newPhase} for building {_buildingConfig.buildingName}");
            if (newPhase == GamePhase.Construction)
            {
                GenerateResources();
            }

            _lastProcessedPhase = newPhase;
        }

        private void GenerateResources()
        {
            if (!_isInitialized)
            {
                Debug.LogWarning($"Cannot generate resources - not initialized on {gameObject.name}");
                return;
            }

            if (_buildingConfig == null)
            {
                Debug.LogError($"BuildingConfig is null when trying to generate resources on {gameObject.name}");
                return;
            }
            
            Debug.Log($"Attempting to generate resources for {_buildingConfig.buildingName}");
            
            if (_buildingConfig.incomingResources == null)
            {
                Debug.LogWarning($"No incoming resources configured for {_buildingConfig.buildingName}");
                return;
            }
            
            if (_buildingConfig.incomingResources.Count == 0)
            {
                Debug.LogWarning($"Empty incoming resources list for {_buildingConfig.buildingName}");
                return;
            }

            if (_resourcesManager == null)
            {
                Debug.LogError($"ResourcesManager is null when trying to generate resources on {gameObject.name}");
                return;
            }
            
            foreach (var resource in _buildingConfig.incomingResources)
            {
                if (resource.config == null)
                {
                    Debug.LogWarning($"Null resource config in incomingResources for {_buildingConfig.buildingName}");
                    continue;
                }

                try
                {
                    Debug.Log($"Generating {resource.amount} of {resource.config.resourceType} for {_buildingConfig.buildingName}");
                    _resourcesManager.AddResource(resource.amount, resource.config.resourceType);
                    
                    // Создаем визуальный эффект получения ресурса
                    if (_visualFactory != null)
                    {
                        _visualFactory.SpawnResourceDropVisual(
                            transform.position + Vector3.up * 2f, // Немного выше здания
                            resource.config.resourceType,
                            resource.amount
                        );
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"Error generating resource {resource.config.resourceType} for {_buildingConfig.buildingName}: {e.Message}\n{e.StackTrace}");
                }
            }
        }
    }
} 