using Game.Manager.Ui.Concrete;
using UnityEngine;

namespace Game.Manager.Ui.Base
{
    public class TapScreen : MonoBehaviour, IScreen
    {
        [SerializeField] private bool animatedScreen;

        #region Animation Settings

        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private float scaleMultiplier = 1f;
        [SerializeField] private float scaleDuration = 1f;
        private Vector3 _defPosition;
        private Vector3 _defScale;

        #endregion

        private void Start()
        {
            Setup();
            AnimatedOpenScreen();
        }

        private void OnDisable()
        {
            //  rectTransform.DOKill();
        }

        private void Setup()
        {
            if (!animatedScreen) return;
            _defPosition = rectTransform.localPosition;
            _defScale = rectTransform.localScale;
        }

        public void OpenScreen()
        {
            if (!gameObject.activeSelf) gameObject.SetActive(true);
        }

        public void AnimatedOpenScreen()
        {
            if (!animatedScreen) return;
            rectTransform.localScale = Vector3.one;
            // rectTransform.DOKill();
            //rectTransform.DOScale(Vector3.one * scaleMultiplier, scaleDuration).SetEase(easeType).SetLoops(-1, LoopType.Yoyo);
        }

        public void CloseScreen()
        {
            if (gameObject.activeSelf) gameObject.SetActive(false);
        }
    }
}
