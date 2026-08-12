using Unity.Cinemachine;
using UnityEngine;

namespace PiratesOnline.Presentation.Cameras
{
    public class CamerasService : MonoBehaviour
    {
        public CinemachineCamera FollowCamera => _followCamera;
        [SerializeField] private CinemachineCamera _followCamera;
    }
}