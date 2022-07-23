using Game.Data.UnityObject;
using Game.Enums;
using Game.Pool;
using UnityEngine;

namespace Game.Model.GameModel
{
    public class GameModel : IGameModel
    {
        private RD_PoolHelper _poolHelper;

        public GameModel()
        {
            Load();
        }

        private void Load()
        {
            Status = Resources.Load<RD_GameStatus>("Data/GameStatus");
            _poolHelper = Resources.Load<RD_PoolHelper>("Data/PoolHelper");
        }

        public RD_GameStatus Status { get; set; }

        public RD_PoolHelper PoolHelper
        {
            get
            {
                if (_poolHelper == null)
                    Load();
                return _poolHelper;
            }
        }

        public GameStatus GetStatus()
        {
            return Status.Value;
        }

        public void Clear()
        {
        }
    }
}
