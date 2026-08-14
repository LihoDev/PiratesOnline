using PiratesOnline.Presentation.Cameras;
using UnityEngine;
using Zenject;

namespace PiratesOnline.Infrastructure.Core
{
    public class SceneInstaller : MonoInstaller
    {
        [SerializeField] private CamerasService _camerasService;

        public override void InstallBindings()
        {
            Container.Bind<CamerasService>().FromInstance(_camerasService).AsSingle();
        }
    }
}