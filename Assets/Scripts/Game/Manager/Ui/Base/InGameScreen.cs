using Game.Level;
using Game.Manager.Ui.Concrete;
using Game.Model.LevelModel;
using Game.Model.PlayerModel;
using Game.Root;
using Game.Signals;
using TMPro;
using UnityEngine;

namespace Game.Manager.Ui.Base
{
    public class InGameScreen : MonoBehaviour, IScreen
    {
        private IPlayerModel _playerModel;
        private ILevelModel _levelModel;
        private GameSignals _gameSignals;
        [SerializeField] private TextMeshProUGUI _playerLifeText;
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private TextMeshProUGUI _levelCoinText;

        private void Awake()
        {
            SetReference();
            SetLifeText();
            SetLevelText();
            SetLevelCoinText();
        }

        private void SetReference()
        {
            _playerModel = GameInstaller.Instance.PlayerModel;
            _levelModel = GameInstaller.Instance.LevelModel;
            _gameSignals = GameInstaller.Instance.GameSignal;
        }


        private void OnEnable()
        {
            _gameSignals.PlayerLifeChange.AddListener(OnPlayerLifeChange);
            _gameSignals.LevelCoinChange.AddListener(OnLevelCoinChange);
        }

        private void OnDisable()
        {
            _gameSignals.PlayerLifeChange.RemoveListener(OnPlayerLifeChange);
            _gameSignals.LevelCoinChange.RemoveListener(OnLevelCoinChange);
        }

        private void OnLevelCoinChange()
        {
            SetLevelCoinText();
        }


        private void OnPlayerLifeChange()
        {
            SetLifeText();
        }


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

        private void SetLifeText()
        {
            _playerLifeText.text = "" + _playerModel.GetLife();
        }

        private void SetLevelText()
        {
            _levelText.text = "LEVEL " + GameData.VisualLevelIndex;
        }

        private void SetLevelCoinText()
        {
            _levelCoinText.text = "" + _levelModel.GetLevelCoin();
        }
    }
}
