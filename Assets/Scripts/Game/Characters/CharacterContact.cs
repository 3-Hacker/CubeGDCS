using Game.Collectables;
using Game.Obstacles;
using Game.Root;
using Game.Signals;
using UnityEngine;

namespace Game.Characters
{
    public class CharacterContact : MonoBehaviour
    {
        private GameSignals _gameSignals;

        private void Start()
        {
            SetReference();
        }

        private void SetReference()
        {
            _gameSignals = GameInstaller.Instance.GameSignal;
        }

        private void OnTriggerEnter(Collider other)
        {
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
