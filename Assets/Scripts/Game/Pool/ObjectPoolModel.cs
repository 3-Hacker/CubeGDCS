using System.Collections.Generic;
using UnityEngine;

namespace Game.Pool
{
    public class ObjectPoolModel : IObjectPoolModel
    {
        private Dictionary<string, ObjectPoolVO> _poolVos;

        private Dictionary<string, Queue<GameObject>> _objectQueues;

        private GameObject _container, _newObj;


        public ObjectPoolModel()
        {
            _poolVos = new Dictionary<string, ObjectPoolVO>();
            _objectQueues = new Dictionary<string, Queue<GameObject>>();
            _container = new GameObject("PoolObjects");
            Object.DontDestroyOnLoad(_container);
        }

        public void Pool(string key, GameObject prefab, int count)
        {
            if (prefab.GetComponent<IPoolable>() == null)
            {
                Debug.LogError("You cant create " + prefab.name + ". IPoolable class is missing");
                return;
            }

            var vo = new ObjectPoolVO
            {
                Key = key,
                Count = count,
                Prefab = prefab
            };

            if (!_poolVos.ContainsKey(vo.Key))
                _poolVos.Add(vo.Key, vo);

            Queue<GameObject> queue;

            if (!_objectQueues.ContainsKey(vo.Key))
            {
                queue = new Queue<GameObject>();
                _objectQueues.Add(vo.Key, queue);
            }
            else
            {
                queue = _objectQueues[vo.Key];
            }

            for (var i = 0; i < vo.Count; i++)
            {
                _newObj = GameObject.Instantiate(vo.Prefab, _container.transform);
                _newObj.GetComponent<IPoolable>().PoolKey = key;
                _newObj.name = vo.Key;
                _newObj.SetActive(false);
                queue.Enqueue(_newObj);
            }
        }

        public void HideAll()
        {
            foreach (KeyValuePair<string, Queue<GameObject>> pair in _objectQueues)
            {
                foreach (GameObject gameObject in pair.Value)
                {
                    gameObject.SetActive(false);
                }
            }
        }


        public GameObject Get(string key, Transform parent)
        {
            var item = Get(key, true);
            item.transform.SetParent(parent, false);
            item.GetComponent<IPoolable>().OnGetFromPool();
            return item;
        }

        public GameObject Get(string key, bool withParent = false)
        {
            if (!_objectQueues.ContainsKey(key))
            {
                Debug.LogWarning("Not object in pool with key " + key);
                return null;
            }

            if (_objectQueues[key].Count == 0)
            {
                Debug.LogWarning("Not enough object in pool with key " + key + ". Instantiating.");
                ObjectPoolVO vo = _poolVos[key];
                Pool(vo.Key, vo.Prefab, 1);
            }

            _newObj = _objectQueues[key].Dequeue();
            _newObj.SetActive(true);
            if (!withParent)
                _newObj.GetComponent<IPoolable>().OnGetFromPool();
            return _newObj;
        }

        public void Return(GameObject obj)
        {
            if (obj.GetComponent<IPoolable>() == null)
            {
                Debug.LogError("You cant destroy " + obj.name + ". IPoolable class is missing");
                return;
            }

            if (!obj.activeInHierarchy)
                return;

            obj.GetComponent<IPoolable>().OnReturnFromPool();
            obj.transform.SetParent(_container.transform);
            obj.SetActive(false);
            _objectQueues[obj.GetComponent<IPoolable>().PoolKey].Enqueue(obj);
        }

        public bool Has(string key)
        {
            return _poolVos.ContainsKey(key);
        }
    }
}
