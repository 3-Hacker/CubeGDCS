using Core.Utils;
using Game.Collectables;
using Game.Enums;
using Game.Model.LevelModel;
using Game.Model.PlayerModel;
using Game.Obstacles;
using Game.Root;
using Game.Signals;
using UnityEngine;

namespace Game.Characters
{
    public class ContactController : MonoBehaviour
    {
        private IPlayerModel _playerModel;
        private ILevelModel _levelModel;
        private GameSignals _gameSignals;

        private void Awake()
        {
            SetReference();
        }

        private void Start()
        {
            _playerModel = GameInstaller.Instance.PlayerModel;
            _levelModel = GameInstaller.Instance.LevelModel;
        }

        private void SetReference()
        {
            _gameSignals = GameInstaller.Instance.GameSignal;
        }

        private void OnEnable()
        {
            _gameSignals.characterCollectableContact.AddListener(OnCharacterCollectableContact);
            _gameSignals.characterObstacleContact.AddListener(OnCharacterObstacleContact);
        }

        private void OnDisable()
        {
            _gameSignals.characterCollectableContact.RemoveListener(OnCharacterCollectableContact);
            _gameSignals.characterObstacleContact.RemoveListener(OnCharacterObstacleContact);
        }


        private void OnCharacterObstacleContact(Obstacle hitObstacle)
        {
            Debug.Log("OnCharacterObstacleContact");
            _playerModel.DecreaseLive();
            //Debug.Log($"PlayerModel {_playerModel.GetLife()}");
        }

        private void OnCharacterCollectableContact(Collectable hitCollectable)
        {
            Debug.Log("OnCharacterCollectableContact");

            if (hitCollectable.GetCollectableType() == CollectableType.X1)
            {
                _levelModel.SetLevelCoin(1);
            }
            else if (hitCollectable.GetCollectableType() == CollectableType.X5)
            {
                _levelModel.SetLevelCoin(5);
            }

            hitCollectable.CloseCollectable(); //todo poolModel.Return(hitCollectable); <-Pool
            _gameSignals.LevelCoinChange.Dispatch();
        }
    }
}
