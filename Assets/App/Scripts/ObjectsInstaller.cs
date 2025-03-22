using UnityEngine;
using Zenject;

namespace App.Scripts
{
    public class ObjectsInstaller : MonoInstaller
    {
        [Header("UI"),SerializeField] private RectTransform windowsContainer;
        
        public override void InstallBindings()
        {
            Container.Bind<RectTransform>().WithId("WindowsContainer").FromInstance(windowsContainer).AsSingle();
        }
    }
}