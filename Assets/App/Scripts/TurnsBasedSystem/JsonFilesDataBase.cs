using UnityEngine;

namespace App.Scripts.TurnsBasedSystem
{
    [CreateAssetMenu(fileName = "FilesDataBaseConfig", menuName = "Configs/DataBases/FilesDataBaseConfig", order = 0)]
    public class JsonFilesDataBase : ScriptableObject
    {
        [SerializeField] public TextAsset BuildingsJsonFile;
        [SerializeField] public TextAsset WavesJsonFile;
    }
}