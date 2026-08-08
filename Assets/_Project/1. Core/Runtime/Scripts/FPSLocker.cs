using UnityEngine;
using Zenject;

namespace Liho.DDD.Presentation.FPS
{
    public class FPSLocker : IInitializable
    {
        public void Initialize()
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;
            Debug.Log($"Set Target Frame Rate {Application.targetFrameRate}");
        }
    }
}