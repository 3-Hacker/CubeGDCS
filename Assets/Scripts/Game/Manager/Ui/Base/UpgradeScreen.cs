﻿using System;
using System.Collections;
using Core.Utils;
using Game.Manager.Ui.Concrete;
using Game.Model.LevelModel;
using Game.Model.PlayerModel;
using Game.Root;
using Game.Signals;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Manager.Ui.Base
{
    public class UpgradeScreen : MonoBehaviour, IScreen
    {
        private RectTransform _rectTransform;
        private GameSignals _gameSignals;
        private ILevelModel _levelModel;
        private IPlayerModel _playerModel;

        [SerializeField] private TextMeshProUGUI _playerTotalCoinText;
        [SerializeField] private TextMeshProUGUI _playerLifeText;
        [SerializeField] private TextMeshProUGUI _playerCollectableValueText;

        [SerializeField] private Button _lifeSellButton;
        [SerializeField] private Button _collectableValueSellButton;

        [SerializeField] private float _upgradeOpenDurationTime = 3f;
        private bool _isOpen;
        private Coroutine _openCoroutine;
        private Coroutine _closeCoroutine;

        private void Awake()
        {
            _gameSignals = GameInstaller.Instance.GameSignal;
        }


        private void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
            _levelModel = GameInstaller.Instance.LevelModel;
            _playerModel = GameInstaller.Instance.PlayerModel;

            SetLevelData();
        }

        private void OnEnable()
        {
            _lifeSellButton.onClick.AddListener(OnLifeSellButton);
            _collectableValueSellButton.onClick.AddListener(OnCollectableValueSellButton);
            _gameSignals.ShopButton.AddListener(OnShopButton);
        }

        private void OnDisable()
        {
            _lifeSellButton.onClick.RemoveListener(OnLifeSellButton);
            _collectableValueSellButton.onClick.RemoveListener(OnCollectableValueSellButton);
            _gameSignals.ShopButton.RemoveListener(OnShopButton);
        }

        private void OnShopButton()
        {
            if (!_isOpen)
            {
                if(_closeCoroutine!=null) StopCoroutine(_closeCoroutine);
                _openCoroutine = StartCoroutine(OpenPanel());
            }
            else
            {
                if(_openCoroutine!=null) StopCoroutine(_openCoroutine);
                _closeCoroutine = StartCoroutine(ClosePanel());
            }
        }

        private void OnCollectableValueSellButton()
        {
        }

        private void OnLifeSellButton()
        {
        }

        public void OpenScreen()
        {
            if (!gameObject.activeSelf) gameObject.SetActive(true);
        }

        [Button(nameof(AnimatedOpenScreen))] public bool AnimatedOpenButton;

        public void AnimatedOpenScreen()
        {
            StartCoroutine(OpenPanel());
        }

        [Button(nameof(AnimateCloseScreen))] public bool AnimatedCloseButton;

        public void AnimateCloseScreen()
        {
            StartCoroutine(ClosePanel());
        }

        private IEnumerator OpenPanel()
        {
            _gameSignals.OpenUpgradePanel.Dispatch();
            _isOpen = true;

            var time = 0f;

            while (true)
            {
                if (time < _upgradeOpenDurationTime)
                {
                    time += Time.deltaTime;
                    _rectTransform.anchoredPosition = Vector2.Lerp(_rectTransform.anchoredPosition, Vector2.zero,
                        time);
                }
                else
                {
                    yield break;
                }

                yield return new WaitForEndOfFrame();
            }
        }

        private IEnumerator ClosePanel()
        {
            _gameSignals.CloseUpgradePanel.Dispatch();
            _isOpen = false;

            var time = 0f;

            while (true)
            {
                if (time < _upgradeOpenDurationTime)
                {
                    time += Time.deltaTime;
                    _rectTransform.anchoredPosition = Vector2.Lerp(_rectTransform.anchoredPosition,
                        new Vector2(-900f, 0f),
                        time);
                }
                else
                {
                    yield break;
                }

                yield return new WaitForEndOfFrame();
            }
        }

        public void CloseScreen()
        {
            if (gameObject.activeSelf) gameObject.SetActive(false);
        }

        public void SetLevelData()
        {
            _playerTotalCoinText.text = "TOTAL COIN :" + _levelModel.GetTotalCoin();
            _playerLifeText.text = "LIFE :" + _playerModel.GetLife();
            _playerCollectableValueText.text = "COLLECTABLE VALUE :" + _levelModel.GetCollectableValue();
        }
    }
}