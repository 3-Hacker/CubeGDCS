using Core.Utils;
using Game.Enums;
using UnityEngine;

namespace Game.Data.UnityObject
{
    [CreateAssetMenu(menuName = "Runtime Data/Game Status", order = 1)]
    public class RD_GameStatus : ScriptableObject
    {
        [EnumFlags] public GameStatus Value;


        public void Block()
        {
            Value = GameStatus.Blocked;
        }

        public void UnBlock()
        {
            Value = GameStatus.Blocked;
        }

        public void Game()
        {
            Value = GameStatus.Game;
        }

        public void Reset()
        {
            Value = GameStatus.None;
        }
    }
}
