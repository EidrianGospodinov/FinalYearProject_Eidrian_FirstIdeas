using Unity.Cinemachine;
using UnityEngine;

namespace _Scripts.Units.Player.Core
{
    public class CameraManager : MonoBehaviour, ICameraService
    {
        public Camera MainCamera => mainCam;
        [SerializeField] private Camera mainCam;
        [SerializeField] private CinemachineCamera freeLookCam;
        [SerializeField] private CinemachineCamera deathCam;

        public void EnableDeathCam(bool enable)
        {
            deathCam.Priority = enable ? 20 : 0;
        }

        public void SetTarget(Transform target)
        {
            freeLookCam.Follow = target;
            deathCam.Follow = target;
        }
    }
    public interface ICameraService
    {
        Camera MainCamera { get; }
        void SetTarget(Transform target);
        void EnableDeathCam(bool enable);
    }
}