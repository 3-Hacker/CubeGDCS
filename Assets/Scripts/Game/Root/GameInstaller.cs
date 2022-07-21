using Game.InputHandler;
using Game.Level;
using Game.Manager;
using Game.Model.GameModel;
using Game.Pool;
using Game.Signals;
using UnityEngine;

namespace Game.Root
{
    public class GameInstaller : MonoSingleton<GameInstaller>
    {
        public GameSignals GameSignal = new GameSignals();
        public IGameModel GameModel;
        public IObjectPoolModel PoolModel;
        public GameManager GameManager;
        public UiManager UIManager;
        public LevelLoader LevelLoader;
        public InputMouseHandler InputMouseHandler;

        private void Awake()
        {
            Debug.Log("GameInstaller - Awake");
            DependLoad();
        }

        private void DependLoad()
        {
            GameModel = new GameModel();
            PoolModel = new ObjectPoolModel();
            ReferenceControl();
        }

        private void ReferenceControl()
        {
            if (GameManager == null) FindObjectOfType<GameManager>();
            if (UIManager == null) GameManager.GetComponent<UiManager>();
            if (LevelLoader == null) GameManager.GetComponent<LevelLoader>();
            if (InputMouseHandler == null) GameManager.GetComponent<InputMouseHandler>();
        }
    }
}
