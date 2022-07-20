using UnityEngine;

namespace Game.Pool
{
    public class Poolable : MonoBehaviour, IPoolable
    {
        public void OnGetFromPool()
        {
        }

        public void OnReturnFromPool()
        {
        }

        public string PoolKey { get; set; }
    }
}
