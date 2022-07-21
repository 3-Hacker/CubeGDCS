using Game.Data.UnityObject;
using UnityEngine;

namespace Game.Model.LevelModel
{
    public class LevelModel : ILevelModel
    {
        private RD_LevelData _levelData;

        public LevelModel()
        {
            _levelData = Resources.Load<RD_LevelData>("Data/LevelData");
        }

        public int GetLevelCoin()
        {
            return _levelData.LevelCoin;
        }

        public void SetLevelCoin(int amount)
        {
            _levelData.LevelCoin += amount;
        }
    }
}
