using System;
using Game.Enums;
using Game.Model.GameModel;
using Game.Root;
using UnityEngine;

namespace Game.InputHandler
{
    public class InputMouseHandler : MonoBehaviour
    {
        private IGameModel _gameModel;

        //[Inject] private IPlayerModel _playerModel;
        private Vector2 _startPos;
        private Vector2 _lastPos;

        public static Action<Vector2> OnDown;
        public static Action<Vector2> OnDrag;
        public static Action OnEnd;

        private void Start()
        {
            _gameModel = GameInstaller.Instance.GameModel;
        }

        private void Update()
        {
            if (_gameModel.Status.Value == GameStatus.Blocked) return;

            #region MovementInput

            if (Input.GetMouseButtonDown(0))
            {
                _startPos = Input.mousePosition;
                //_lastPos = _startPos;
                OnDown?.Invoke(_lastPos - _startPos);
            }
            else if (UnityEngine.Input.GetMouseButton(0))
            {
                var pos = (Vector2)Input.mousePosition - _lastPos;
                pos.x /= Screen.height;
                pos.y /= Screen.height;
                pos *= 1500; //_playerModel.GetInputSensitive();


                OnDrag?.Invoke(pos);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                _lastPos = Input.mousePosition;
                OnEnd?.Invoke();
            }

            _lastPos = Input.mousePosition;

            #endregion
        }
    }
}
