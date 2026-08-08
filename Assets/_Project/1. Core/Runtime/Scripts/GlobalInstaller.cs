using PiratesOnline.Domain.Service;
using PiratesOnline.Presentation.FPS;
using UnityEngine;
using Zenject;

namespace PiratesOnline.Infrastructure.Core
{
    public class GlobalInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IServerDataService>().To<MockServerDataService>().AsSingle();
            Container.BindInterfacesTo<FPSLocker>().AsSingle();
        }
    }
}