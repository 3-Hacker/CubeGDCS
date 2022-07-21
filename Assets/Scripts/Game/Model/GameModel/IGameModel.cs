using Game.Data.UnityObject;
using Game.Enums;
using Game.Pool;

namespace Game.Model.GameModel
{
    public interface IGameModel
    {
        RD_GameStatus Status { get; }
        RD_PoolHelper PoolHelper { get; }
        GameStatus GetStatus();

        void Clear();
    }
}
