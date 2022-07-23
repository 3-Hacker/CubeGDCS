using Game.Upgrade;
using UnityEngine;

namespace Game.Data.UnityObject
{
    [CreateAssetMenu(menuName = "Runtime Data/Level Data", order = 0)]
    public class RD_LevelData : ScriptableObject
    {
        public int TotalCoin;
        public int LevelCoin;
        public int CollectableMultiplyCoin;
        public Cost CollectableCost;
    }
}
