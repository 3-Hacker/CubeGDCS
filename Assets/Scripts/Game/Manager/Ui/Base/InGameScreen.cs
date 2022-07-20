using Game.Manager.Ui.Concrete;
using UnityEngine;

namespace Game.Manager.Ui.Base
{
    public class InGameScreen : MonoBehaviour, IScreen
    {
        public void OpenScreen()
        {
            if (!gameObject.activeSelf) gameObject.SetActive(true);
        }

        public void AnimatedOpenScreen()
        {
            
        }

        public void CloseScreen()
        {
            if (gameObject.activeSelf) gameObject.SetActive(false);
        }
    }
}
