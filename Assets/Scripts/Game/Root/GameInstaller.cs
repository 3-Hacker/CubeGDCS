using System;
using Game.InputHandler;
using Game.Level;
using Game.Manager;
using Game.Model.GameModel;
using Game.Model.PlayerModel;
using Game.Pool;
using Game.Signals;
using UnityEngine;

namespace Game.Root
{
    public class GameInstaller : MonoSingleton<GameInstaller>
    {
        public GameSignals GameSignal = new GameSignals();
        public IPlayerModel PlayerModel;
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

        private void Start()
        {
            ReferenceControl();
        }

        private void DependLoad()
        {
            GameModel = new GameModel();
            PlayerModel = new PlayerModel();
            PoolModel = new ObjectPoolModel();
        }

        private void ReferenceControl()
        {
            if (GameManager == null) GameManager = FindObjectOfType<GameManager>();
            if (UIManager == null) UIManager = GameManager.GetComponent<UiManager>();
            if (LevelLoader == null) LevelLoader = GameManager.GetComponent<LevelLoader>();
            if (InputMouseHandler == null) InputMouseHandler = GameManager.GetComponent<InputMouseHandler>();
        }
    }
}
