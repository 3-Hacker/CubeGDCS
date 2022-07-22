using Game.Data.UnityObject;
using Game.Root;
using Game.Signals;
using UnityEngine;

namespace Game.Model.PlayerModel
{
    public class PlayerModel : IPlayerModel
    {
        private RD_PlayerData _playerData;
        private GameSignals _gameSignals;
        
        public PlayerModel()
        {
            _playerData = Resources.Load<RD_PlayerData>("Data/PlayerData");
            _gameSignals = GameInstaller.Instance.GameSignal;
        }


        public float GetSensitivity()
        {
            return _playerData.PlayerInputSensitivity;
        }

        public float GetSpeed()
        {
            return _playerData.PlayerSpeed;
        }

        public float GetTurnSpeed()
        {
            return _playerData.PlayerTurnSpeed;
        }

        public bool GetSharpMode()
        {
            return _playerData.SharpMode;
        }


        public float GetMaxXPos()
        {
            return _playerData.PlayerMaxXPos;
        }

        public int GetLife()
        {
            return _playerData.PlayerLife;
        }

        public void DecreaseLive()
        {
            if (_playerData.PlayerLife > 0)
            {
                _playerData.PlayerLife--;
                _gameSignals.PlayerLifeChange.Dispatch();
            }

            if (_playerData.PlayerLife == 0)
            {
                _gameSignals.PlayerDead.Dispatch();
            }
        }
    }
}
