namespace Game.Model.LevelModel
{
    public interface ILevelModel
    {
        public int GetLevelCoin();
        public int GetTotalCoin();
        public void DecreaseTotalCoin();
        public int GetCollectableValue();
        public void SetTotalCoin();
        public void SetLevelCoin(int multiplier);
        public bool IsSellCollectable();
        public void SetCollectableValue();
        public int GetCollectableCostValue();
        public void DecreaseLevelCoin();
        public void IncreaseTotalCoin();
    }
}
