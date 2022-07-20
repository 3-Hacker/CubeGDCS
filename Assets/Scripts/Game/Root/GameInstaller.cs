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
        public LevelLoader LevelLoader;


        private void Awake()
        {
            Debug.Log("GameInstaller - Awake");
            DependLoad();
        }

        private void DependLoad()
        {
            GameModel = new GameModel();
            PoolModel = new ObjectPoolModel();
        }
    }
}
