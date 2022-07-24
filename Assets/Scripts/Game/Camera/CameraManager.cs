using Cinemachine;
using Game.Root;
using Game.Signals;
using UnityEngine;

namespace Game.Camera
{
    public class CameraManager : MonoBehaviour
    {
        private GameSignals _gameSignals;
        [SerializeField] private CinemachineBrain cinemachineBrain;
        [SerializeField] private CinemachineVirtualCamera _gameEndCam;
        protected int VirtualCameraPriorty = 30;

        private void Start()
        {
            _gameSignals = GameInstaller.Instance.GameSignal;
            _gameSignals.LevelFinish.AddListener(OnLevelFinish);
        }


        private void OnDisable()
        {
            _gameSignals.LevelFinish.RemoveListener(OnLevelFinish);
        }

        private void OnLevelFinish()
        {
            GameEndCam();
        }


        protected virtual int ChangeCameraPriority()
        {
            return ++VirtualCameraPriorty;
        }

        private void GameEndCam(float cameraBlendTime = 0.5f)
        {
            cinemachineBrain.m_DefaultBlend.m_Time = cameraBlendTime;
            _gameEndCam.Priority = ChangeCameraPriority();
        }
    }
}
