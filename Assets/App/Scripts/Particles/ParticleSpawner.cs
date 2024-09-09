using App.Scripts.Buildings;
using UnityEngine;

namespace App.Scripts.Particles
{
    public class ParticleSpawner : MonoBehaviour
    {
        [SerializeField] private ParticleSystem placingParticle;

        public void SpawnParticleAtObject(Building building, Vector3 position)
        {
            var particleInstance = Instantiate(placingParticle, position, Quaternion.identity);

            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new[] { new GradientColorKey(building.BuildingConfig.buildingAssociateColor, 1.0f) },
                new[] { new GradientAlphaKey(1.0f, 1.0f) }
            );
            
            var colorOverLifetime = particleInstance.colorOverLifetime;
            
            var colorModule = colorOverLifetime;
            
            colorModule.color = gradient;
        }
    }
}