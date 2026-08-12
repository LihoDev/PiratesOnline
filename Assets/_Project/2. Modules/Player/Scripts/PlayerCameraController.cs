using Mirror;
using PiratesOnline.Presentation.Cameras;
using Zenject;

namespace PiratesOnline.Presentation.Player
{
    public class PlayerCameraController : NetworkBehaviour
    {
        private CamerasService _cameras;

        [Inject]
        public void Construct(CamerasService cameras)
        {
            _cameras = cameras;
        }

        //private void Awake()
        //{
        //    ProjectContext.Instance.Container.Inject(this);
        //}

        public override void OnStartLocalPlayer()
        {
            if (_cameras != null)
            {
                _cameras.FollowCamera.Follow = gameObject.transform;
            }
        }
    }
}