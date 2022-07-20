using Game.Data.UnityObject;
using Game.Pool;

namespace Game.Model.GameModel
{
    public interface IGameModel
    {
        RD_GameStatus Status { get; }
        RD_PoolHelper PoolHelper { get; }
        void Clear();
    }
}
