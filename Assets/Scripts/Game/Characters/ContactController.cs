using System;
using Game.Collectables;
using Game.Obstacles;
using Game.Root;
using Game.Signals;
using UnityEngine;

namespace Game.Characters
{
    public class ContactController : MonoBehaviour
    {
        private GameSignals _gameSignals;

        private void Awake()
        {
            SetReference();
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
        }

        private void OnCharacterCollectableContact(Collectable hitCollectable)
        {
            Debug.Log("OnCharacterCollectableContact");
        }
    }
}
