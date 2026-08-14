using Mirror;
using Zenject;

namespace PiratesOnline.Infrastructure.Network
{
    public class PlayerNetworkInjecter : NetworkBehaviour
    {
        [Inject] private DiContainer Container;
        private bool _isInjected = false;

        public override void OnStartServer()
        {
            base.OnStartServer();
            if (!_isInjected)
            {
                Container.InjectGameObject(gameObject);
                _isInjected = true;
            }
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            if (!_isInjected)
            {
                Container.InjectGameObject(gameObject);
                _isInjected = true;
            }
        }
    }
}