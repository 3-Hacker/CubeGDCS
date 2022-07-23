namespace Game.Model.LevelModel
{
    public interface ILevelModel
    {
        public int GetLevelCoin();
        public int GetTotalCoin();
        public int GetCollectableValue();
        public void SetTotalCoin();
        public void SetLevelCoin(int multiplier);
        public void SetCollectableValue();
    }
}
