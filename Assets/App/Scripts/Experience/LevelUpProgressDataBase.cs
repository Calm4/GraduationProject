using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace App.Scripts
{
    [CreateAssetMenu(fileName = "LevelUpProgressDataBase", menuName = "Configs/DataBases/LevelUpProgressDataBase", order = 0)]
    public class LevelUpProgressDataBase : SerializedScriptableObject
    {
        // level|level-experience
        [OdinSerialize] public Dictionary<int,int> LevelProgressData = new(); 
    }
}