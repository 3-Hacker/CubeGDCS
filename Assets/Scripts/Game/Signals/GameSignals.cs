using Game.Collectables;
using Game.Obstacles;

namespace Game.Signals
{
    public class GameSignals
    {
        //Pool
        public readonly Signal<bool> initPool = new Signal<bool>();

        //GameStart
        public readonly Signal gameStart = new Signal();

        //Contact
        public readonly Signal<Obstacle> characterObstacleContact = new Signal<Obstacle>();
        public readonly Signal<Collectable> characterCollectableContact = new Signal<Collectable>();

        //Player
        public readonly Signal PlayerDead = new Signal();
        public readonly Signal PlayerLifeChange = new Signal();
        public readonly Signal LevelCoinChange = new Signal();
        
        //Upgrade
        public readonly Signal OpenUpgradePanel = new Signal();
        public readonly Signal CloseUpgradePanel = new Signal();
        
        //Shop
        public readonly Signal ShopButton = new Signal();


    }
}
