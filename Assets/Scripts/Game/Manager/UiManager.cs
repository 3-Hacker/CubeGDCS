using Game.Enums;
using Game.Level;
using Game.Manager.Ui.Base;
using Game.Model.GameModel;
using Game.Root;
using Game.Signals;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Manager
{
    public class UiManager : MonoBehaviour
    {
        private GameManager _gameManager;
        private IGameModel _gameModel;
        private GameSignals _gameSignals;
        private LevelLoader _levelLoader;

        private bool _initPool;

        #region Panels

        [SerializeField] private TapScreen TapScreen;
        [SerializeField] private InGameScreen InGameScreen;
        [SerializeField] private SuccessScreen SuccessScreen;
        [SerializeField] private FailScreen FailScreen;

        #endregion


        #region Buttons

        [SerializeField] private Button TapButton;
        [SerializeField] private Button SuccessButton;
        [SerializeField] private Button FailButton;

        #endregion

        #region Config Settings

        private void Awake()
        {
            SetReference();
        }

        private void SetReference()
        {
            _gameModel = GameInstaller.Instance.GameModel;
            _gameManager = GameInstaller.Instance.GameManager;
            _gameSignals = GameInstaller.Instance.GameSignal;
            _levelLoader = GameInstaller.Instance.LevelLoader;
        }

        private void OnEnable()
        {
            TapButton.onClick.AddListener(OnTapClick);
            SuccessButton.onClick.AddListener(OnSuccessClick);
            FailButton.onClick.AddListener(OnFailClick);
            _gameSignals.initPool.AddListener(OnInitPool);
            _gameSignals.PlayerDead.AddListener(OnPlayerDead);
        }

        private void OnDisable()
        {
            _gameSignals.initPool.RemoveListener(OnInitPool);
            TapButton.onClick.RemoveListener(OnTapClick);
            SuccessButton.onClick.RemoveListener(OnSuccessClick);
            FailButton.onClick.RemoveListener(OnFailClick);
            _gameSignals.PlayerDead.RemoveListener(OnPlayerDead);
        }

        private void OnPlayerDead()
        {
            Invoke(nameof(OpenFailScreen), 1f);
        }

        private void OnTapClick()
        {
            GameStart();
        }


        private void OnFailClick()
        {
            _levelLoader.RestartScene();
        }

        private void OnSuccessClick()
        {
            _levelLoader.NextLevel();
        }


        private void OnInitPool(bool value)
        {
            _initPool = value;
        }

        #endregion

        #region Panel Settings

        private void GameStart()
        {
            if (!_initPool)
            {
                Debug.Log("Pool Hazır Değil");
                return;
            }

            _gameManager.GameStart();
            TapScreen.CloseScreen();
            InGameScreen.OpenScreen();
        }

        public void OpenSuccessScreen()
        {
            TapScreen.CloseScreen();
            InGameScreen.CloseScreen();
            SuccessScreen.OpenScreen();
        }

        public void OpenFailScreen()
        {
            TapScreen.CloseScreen();
            InGameScreen.CloseScreen();
            FailScreen.OpenScreen();
        }

        #endregion
    }
}
