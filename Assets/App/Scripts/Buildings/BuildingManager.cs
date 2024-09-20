using App.Scripts.Animations;
using App.Scripts.Particles;
using DG.Tweening;
using UnityEngine;

namespace App.Scripts.Buildings
{
    public class BuildingManager : MonoBehaviour
    {
        [Space(20)] [SerializeField] private ParticleSpawner particleSpawner;
        [SerializeField] private Transform buildingsContainer;
        [SerializeField] private Vector3 buildingOffsetPoint;

        [Space(20)] [SerializeField] private AnimationsConfig animationsConfig;
        [SerializeField] private ParticleSystem placingParticle;

        public Building CreateBuilding(Building buildingPrefab)
        {
            Building building = Instantiate(buildingPrefab, buildingsContainer);
            building.Initialize(buildingPrefab.BuildingConfig);

            return building;
        }

        public Building PlaceBuilding(Building buildingPrefab, Vector3 position)
        {
            Building building = CreateBuilding(buildingPrefab);
            building.transform.position = position + buildingOffsetPoint;

            Vector3 buildingCenterOffset =
                new Vector3(building.BuildingConfig.size.x / 2f, 0, building.BuildingConfig.size.y / 2f);
            Vector3 particlePosition = position + buildingCenterOffset;

            building.transform.DOMove(position, animationsConfig.buildingPlacingTime).SetEase(Ease.InCirc)
                .OnComplete(() => particleSpawner.SpawnParticleAtObject(building, particlePosition));
            return building;
        }

        public void RemoveBuilding(Building building)
        {
            if (!building)
                Destroy(building.gameObject);
        }
    }
}