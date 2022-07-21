using Game.Data.UnityObject;
using UnityEngine;

namespace Game.Model.PlayerModel
{
    public class PlayerModel : IPlayerModel
    {
        private RD_PlayerData _playerData;

        public PlayerModel()
        {
            _playerData = Resources.Load<RD_PlayerData>("Data/PlayerData");
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
    }
}
