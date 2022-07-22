using System;
using Core.Utils;
using Game.Collectables;
using Game.Enums;
using Game.Model.GameModel;
using Game.Obstacles;
using Game.Root;
using Game.Signals;
using UnityEngine;

namespace Game.Characters
{
    public class CharacterContact : MonoBehaviour
    {
        [SerializeField] private RagdollBehaviour _ragdollBehaviour;

        private GameSignals _gameSignals;
        private IGameModel _gameModel;

        private void Awake()
        {
            SetReference();
        }

        private void Start()
        {
            _gameModel = GameInstaller.Instance.GameModel;
        }

        private void SetReference()
        {
            _gameSignals = GameInstaller.Instance.GameSignal;
            if (_ragdollBehaviour == null) _ragdollBehaviour = GetComponent<RagdollBehaviour>();
        }

        private void OnEnable()
        {
            _gameSignals.PlayerDead.AddListener(OnPlayerDead);
        }

        private void OnDisable()
        {
            _gameSignals.PlayerDead.RemoveListener(OnPlayerDead);
        }

        private void OnPlayerDead()
        {
            _ragdollBehaviour.RagdollOn();
            _gameModel.Status.Block();
        }


        private void OnTriggerEnter(Collider other)
        {
            if (_gameModel.Status.Value == GameStatus.Blocked) return;
            if (other.transform.TryGetComponent(out Obstacle obstacle))
            {
                _gameSignals.characterObstacleContact.Dispatch(obstacle);
            }
            else if (other.transform.TryGetComponent(out Collectable collectables))
            {
                _gameSignals.characterCollectableContact.Dispatch(collectables);
            }
        }
    }
}
