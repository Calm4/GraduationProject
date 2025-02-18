using App.Scripts.Buildings;
using UnityEngine;

namespace App.Scripts
{
    public interface IBuildingButtonInitializer
    {
        public void BaseInitializer(Building perentBuilding);
    }
}