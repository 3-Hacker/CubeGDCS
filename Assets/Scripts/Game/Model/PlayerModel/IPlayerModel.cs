namespace Game.Model.PlayerModel
{
    public interface IPlayerModel
    {
        public float GetSensitivity();
        public float GetSpeed();
        public float GetTurnSpeed();
        public bool GetSharpMode();
        public float GetMaxXPos();
        public int GetLife();
        public void DecreaseLife();
        public void SetLife();
        public bool IsSellLife();
        public bool IsItEnough();
        public  int GetLifeCostValue();
       public void IncreaseLife();
    }
}
