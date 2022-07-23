using Game.Collectables;
using Game.Enums;
using Game.Obstacles;

namespace Game.Signals
{
    public class GameSignals
    {
        //Pool
        public readonly Signal<bool> InitPool = new Signal<bool>();

        //GameStart
        public readonly Signal GameStart = new Signal();

        //GameState
        public readonly Signal<GameStatus> SetGameState = new Signal<GameStatus>();

        //Contact
        public readonly Signal<Obstacle> CharacterObstacleContact = new Signal<Obstacle>();
        public readonly Signal<Collectable> CharacterCollectableContact = new Signal<Collectable>();

        //Player
        public readonly Signal PlayerDead = new Signal();
        public readonly Signal PlayerVictory = new Signal();
        public readonly Signal PlayerLifeChange = new Signal();
        public readonly Signal LevelCoinChange = new Signal();

        //Upgrade
        public readonly Signal OpenUpgradePanel = new Signal();
        public readonly Signal CloseUpgradePanel = new Signal();

        //Shop
        public readonly Signal ShopButton = new Signal();

        //Level
        public readonly Signal LevelFinish = new Signal();
    }
}
