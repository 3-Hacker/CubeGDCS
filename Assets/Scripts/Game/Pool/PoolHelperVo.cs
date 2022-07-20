using UnityEngine;

namespace Game.Pool
{
    [System.Serializable]
    public class PoolHelperVo
    {
        public PoolKey Key;
        public int Count;
        public GameObject Prefab;
    }
}
