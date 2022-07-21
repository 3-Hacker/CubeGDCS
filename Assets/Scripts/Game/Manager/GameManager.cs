using Game.Model.GameModel;
using Game.Pool;
using Game.Root;
using Game.Signals;
using UnityEngine;

namespace Game.Manager
{
    public class GameManager : MonoBehaviour
    {
        private IObjectPoolModel _poolModel { get; set; }
        private GameSignals _gameSignals { get; set; }
        private IGameModel _gameModel { get; set; }

        private void Start()
        {
            Set();
            InitPool();
        }

        private void InitPool()
        {
            for (int i = 0; i < _gameModel.PoolHelper.List.Count; i++)
            {
                var item = _gameModel.PoolHelper.List[i];
                _poolModel.Pool(item.Key.ToString(), item.Prefab, item.Count);
            }

            _gameSignals.InitPool.Dispatch(true);
        }

        private void Set()
        {
            _poolModel = GameInstaller.Instance.PoolModel;
            _gameSignals = GameInstaller.Instance.GameSignal;
            _gameModel = GameInstaller.Instance.GameModel;
            _gameModel.Status.Block();
        }

        public void GameStart()
        {
            _gameModel.Status.Game();
            _gameSignals.GameStart.Dispatch();
        }
    }
}
