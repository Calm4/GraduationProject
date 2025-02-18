using App.Scripts.Buildings;
using App.Scripts.Enemies;
using App.Scripts.Factories;
using App.Scripts.GameResources;
using App.Scripts.Grid;
using App.Scripts.Input;
using App.Scripts.JsonClasses;
using App.Scripts.Particles;
using App.Scripts.Placement;
using App.Scripts.Sound;
using App.Scripts.TurnsBasedSystem;
using App.Scripts.TurnsBasedSystem.Waves;
using UnityEngine;
using Zenject;

namespace App.Scripts
{
    public class GameInstaller : MonoInstaller
    {
        [Header("Core Managers")]
        [SerializeField] private GridManager gridManager;
        [SerializeField] private PlacementManager placementManager;
        [SerializeField] private BuildingManager buildingManager;
        [SerializeField] private ResourcesManager resourcesManager;
        [SerializeField] private TurnsBasedManager turnsBasedManager;
        [SerializeField] private ExperienceManager experienceManager;
        [SerializeField] private WavesManager wavesManager;
        [SerializeField] private GamePhaseManager gamePhaseManager;
        
        [Space(10)] 
        [Header("Meta Managers")]
        [SerializeField] private JsonLoaderManager jsonLoaderManager;
        [SerializeField] private SoundFeedbackManager soundFeedbackManager;
        [SerializeField] private ParticleManager particleManager;
        [SerializeField] private InputManager inputManager;
        
        [Space(10)][Header("UI")]
        [SerializeField] private BuildingButtonsUIPanel buildingButtonsUIPanel;
        [SerializeField] private OpenPanelsManager openPanelsManager;
        
        
        public override void InstallBindings()
        {
            Container.Bind<ExperienceManager>().FromInstance(experienceManager).AsSingle();
            Container.Bind<WavesManager>().FromInstance(wavesManager).AsSingle();
            Container.Bind<GamePhaseManager>().FromInstance(gamePhaseManager).AsSingle();


            Container.Bind<GridManager>().FromInstance(gridManager).AsSingle();
            Container.Bind<PlacementManager>().FromInstance(placementManager).AsSingle();
            Container.Bind<BuildingManager>().FromInstance(buildingManager).AsSingle();
            Container.Bind<ResourcesManager>().FromInstance(resourcesManager).AsSingle();
            Container.Bind<TurnsBasedManager>().FromInstance(turnsBasedManager).AsSingle();
            
            Container.Bind<JsonLoaderManager>().FromInstance(jsonLoaderManager).AsSingle();
            Container.Bind<SoundFeedbackManager>().FromInstance(soundFeedbackManager).AsSingle();
            Container.Bind<ParticleManager>().FromInstance(particleManager).AsSingle();
            Container.Bind<InputManager>().FromInstance(inputManager).AsSingle();
        
            Container.Bind<BuildingButtonsUIPanel>().FromInstance(buildingButtonsUIPanel).AsSingle();
            Container.Bind<OpenPanelsManager>().FromInstance(openPanelsManager).AsSingle();
            
            Container.Bind<IBuildingFactory>().To<BuildingFactory>().AsSingle();
            Container.Bind<IEnemyFactory>().To<EnemyFactory>().AsSingle();
        }
    }
}