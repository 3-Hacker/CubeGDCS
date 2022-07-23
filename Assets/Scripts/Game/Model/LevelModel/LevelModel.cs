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
            ResetData();
        }

        private void ResetData()
        {
            _levelData.LevelCoin = 0;
        }

        public int GetLevelCoin()
        {
            return _levelData.LevelCoin;
        }

        public int GetTotalCoin()
        {
            return _levelData.TotalCoin;
        }

        public int GetCollectableValue()
        {
            return _levelData.CollectableMultiplyCoin;
        }

        public void SetTotalCoin()
        {
            _levelData.TotalCoin += _levelData.LevelCoin;
        }

        public void SetLevelCoin(int multiplier)
        {
            _levelData.LevelCoin += _levelData.CollectableMultiplyCoin * multiplier;
        }

        public void SetCollectableValue()
        {
            _levelData.CollectableMultiplyCoin += 10;
        }
    }
}
