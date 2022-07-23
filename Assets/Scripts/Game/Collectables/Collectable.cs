using Game.Enums;
using UnityEngine;

namespace Game.Collectables
{
    public class Collectable : MonoBehaviour
    {
        [SerializeField] private CollectableType _collectableType;


        public CollectableType GetCollectableType()
        {
            return _collectableType;
        }

        public void CloseCollectable()
        {
            if (gameObject.activeSelf) gameObject.SetActive(false);
        }
    }
}
