using System.Collections.Generic;
using App.Scripts.Buildings;
using App.Scripts.UI.Buttons;
using UnityEngine;

namespace App.Scripts.UI.Windows
{
    public class OpenPanelsManager : MonoBehaviour
    {
        private readonly Dictionary<int, IBuildingButtonInitializer> _openPanels = new Dictionary<int, IBuildingButtonInitializer>();
        
        public void RegisterWindow(Building building, IBuildingButtonInitializer window)
        {
            int buildingId = building.GetInstanceID();
            if (_openPanels.ContainsKey(buildingId))
            {
                IBuildingButtonInitializer existingWindow = _openPanels[buildingId];
                if (existingWindow is MonoBehaviour mono)
                {
                    Destroy(mono.gameObject);
                }
                _openPanels.Remove(buildingId);
            }
            _openPanels.Add(buildingId, window);
        }
        
        public void UnregisterWindow(Building building)
        {
            int buildingId = building.GetInstanceID();
            if (_openPanels.ContainsKey(buildingId))
            {
                _openPanels.Remove(buildingId);
            }
        }
    }
}