using System.Collections.Generic;
using App.Scripts.Buildings.BuildingsConfigs;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace App.Scripts
{
   public class BuildingsSectionsUIPanel : MonoBehaviour
   {
      [SerializeField] private ScrollRect defensiveScrollView;
      [SerializeField] private ScrollRect peacefulScrollView;
      [SerializeField] private ScrollRect neutralScrollView;

      [FormerlySerializedAs("buildingsSectionsElementsSO")] [SerializeField] private BuildingsDataBaseBySectionsSO buildingsDataBaseBySectionsSO;
      
      

   }
}
