using System;
using Game.Manager.Ui.Concrete;
using Game.Root;
using Game.Signals;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Manager.Ui.Base
{
    public class TapScreen : MonoBehaviour, IScreen
    {
        private GameSignals _gameSignals;
        [SerializeField] private bool animatedScreen;
        [SerializeField] private Button _shopButton;

        #region Animation Settings

        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private float scaleMultiplier = 1f;
        [SerializeField] private float scaleDuration = 1f;
        private Vector3 _defPosition;
        private Vector3 _defScale;

        #endregion


        private void Awake()
        {
            _gameSignals = GameInstaller.Instance.GameSignal;
        }

        private void OnEnable()
        {
            _shopButton.onClick.AddListener(OnShopButton);
        }
        
        private void OnDisable()
        {
            _shopButton.onClick.RemoveListener(OnShopButton);
        }


        private void Start()
        {
            Setup();
            AnimatedOpenScreen();
        }

        private void OnShopButton()
        {
            _gameSignals.ShopButton.Dispatch();
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
