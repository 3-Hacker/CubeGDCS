using Game.Enums;
using Game.InputHandler;
using Game.Model.GameModel;
using Game.Model.PlayerModel;
using Game.Root;
using UnityEngine;

namespace Game.Characters
{
    public class MovementController : MonoBehaviour
    {
        private IGameModel _gameModel;
        private IPlayerModel _playerModel;

        private void OnEnable()
        {
            InputMouseHandler.OnDrag += OnMove;
            InputMouseHandler.OnEnd += OnEnd;
        }

        private void OnDisable()
        {
            InputMouseHandler.OnDrag -= OnMove;
            InputMouseHandler.OnEnd -= OnEnd;
        }


        private void Start()
        {
            SetReference();
        }


        private void SetReference()
        {
            _gameModel = GameInstaller.Instance.GameModel;
            _playerModel = GameInstaller.Instance.PlayerModel;
        }

        private void OnMove(Vector2 inputValue)
        {
            if (_gameModel.GetStatus() != GameStatus.Game) return;
            ChangeLine(inputValue);
        }

        private void OnEnd()
        {
            PlayerNotSharpModeEnd();
        }

        private void PlayerNotSharpModeEnd()
        {
            if (!_playerModel.GetSharpMode())
            {
                transform.localEulerAngles = Vector3.zero;
            }
        }


        private void Update()
        {
            if (_gameModel.GetStatus() != GameStatus.Game) return;
            MovementForward();
        }


        private void MovementForward()
        {
            transform.Translate(Vector3.forward * Time.deltaTime * _playerModel.GetSpeed());
        }

        private void ChangeLine(Vector2 inputValue)
        {
            if (_playerModel.GetSharpMode())
            {
                Transform tempTransform;
                (tempTransform = transform).Translate(Vector3.right * inputValue * _playerModel.GetSensitivity());
                var position = tempTransform.position;
                position = new Vector3(Mathf.Clamp(position.x, -2.4f, 2.4f), position.y, position.z);
                transform.position = position;
            }
            else
            {
                Transform tempTransform;
                (tempTransform = transform).Rotate(Vector3.up * inputValue.x * _playerModel.GetTurnSpeed(),
                    Space.World);
                var position = tempTransform.position;
                position = new Vector3(Mathf.Clamp(position.x, -_playerModel.GetMaxXPos(), _playerModel.GetMaxXPos()),
                    position.y, position.z);
                transform.position = position;

                var rotation = transform.eulerAngles;
                rotation.y = CustomClampAngle(rotation.y, -30f, 30f);
                transform.eulerAngles = rotation;
            }
        }

        private float CustomClampAngle(float angle, float from, float to)
        {
            if (angle < 0f) angle = 360 + angle;
            if (angle > 180f) return Mathf.Max(angle, 360 + from);
            return Mathf.Min(angle, to);
        }
    }
}
