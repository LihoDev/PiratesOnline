using PiratesOnline.Domain.Service;
using PiratesOnline.Infrastructure.Input;
using PiratesOnline.Presentation.FPS;
using Zenject;

namespace PiratesOnline.Infrastructure.Core
{
    public class GlobalInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IServerDataService>().To<MockServerDataService>().AsSingle();
            Container.Bind<IAssetProvider>().To<AddressableAssetProvider>().AsSingle();
            Container.Bind<IMapService>().To<MapService>().AsSingle();
            Container.BindInterfacesTo<FPSLocker>().AsSingle();
            Container.BindInterfacesTo<InputManager>().AsSingle();
        }
    }
}