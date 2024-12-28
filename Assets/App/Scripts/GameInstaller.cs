using System.Resources;
using App.Scripts;
using App.Scripts.Buildings;
using App.Scripts.Enemies;
using App.Scripts.GameResources;
using App.Scripts.Grid;
using App.Scripts.Input;
using App.Scripts.JsonClasses;
using App.Scripts.Particles;
using App.Scripts.Placement;
using App.Scripts.Sound;
using App.Scripts.TurnsBasedSystem;
using App.Scripts.TurnsBasedSystem.Waves;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;


public class GameInstaller : MonoInstaller
{
    public GridManager gridManager;
    public PlacementManager placementManager;
    public BuildingManager buildingManager;
    public ResourcesManager resourcesManager;
    public TurnsBasedManager turnsBasedManager;
    [Space(10)] public ExperienceManager experienceManager;
    public WavesManager wavesManager;
    public GamePhaseManager gamePhaseManager;

    [Space(10)] public JsonLoaderManager jsonLoaderManager;
    public SoundFeedbackManager soundFeedbackManager;
    public ParticleManager particleManager;
    public InputManager inputManager;
    
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
    }
}