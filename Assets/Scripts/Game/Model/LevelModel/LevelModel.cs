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

        public void DecreaseTotalCoin()
        {
            if (_levelData.TotalCoin > 0)
            {
                _levelData.TotalCoin--;
            }
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

        public bool IsItEnough()
        {
            return _levelData.TotalCoin > 0 && _levelData.TotalCoin >= _levelData.CollectableCost.CostValue;
        }


        public bool IsSellCollectable()
        {
            return _levelData.CollectableCost.CostValue < _levelData.CollectableCost.MaxCostValue;
        }


        public void SetCollectableValue()
        {
            _levelData.TotalCoin -= _levelData.CollectableCost.CostValue;
            _levelData.CollectableCost.CostValue *= 2;
            _levelData.CollectableMultiplyCoin++;
        }

        public int GetCollectableCostValue()
        {
            return _levelData.CollectableCost.CostValue;
        }

        public void DecreaseLevelCoin()
        {
            _levelData.LevelCoin--;
        }

        public void IncreaseTotalCoin()
        {
            _levelData.TotalCoin++;
        }
    }
}
