using System.Collections;
using Core.Utils;
using Game.Manager.Ui.Concrete;
using Game.Model.LevelModel;
using Game.Root;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Manager.Ui.Base
{
    public class SuccessScreen : MonoBehaviour, IScreen
    {
        private ILevelModel _levelModel;
        [SerializeField] private Button _nextButton;
        [SerializeField] private TextMeshProUGUI _totalCoinText;
        [SerializeField] private TextMeshProUGUI _levelCoinText;


        private void Start()
        {
            _levelModel = GameInstaller.Instance.LevelModel;
            SetTotalCoinText();
            SetLevelCoinText();
        }

        public void OpenScreen()
        {
            if (!gameObject.activeSelf) gameObject.SetActive(true);
            Invoke(nameof(SetLevelCoin), 1f);
        }

        public void AnimatedOpenScreen()
        {
        }

        public void CloseScreen()
        {
            if (gameObject.activeSelf) gameObject.SetActive(false);
        }

        [Button(nameof(SetLevelCoin))] public Button SetLevelCoinButton;

        public void SetLevelCoin()
        {
            StartCoroutine(DecreaseCoroutine());
        }

        private IEnumerator DecreaseCoroutine()
        {
            var loopCount = _levelModel.GetLevelCoin();

            for (int i = 0; i < loopCount; i++)
            {
                _levelModel.DecreaseLevelCoin();
                _levelModel.IncreaseTotalCoin();
                yield return new WaitForSeconds(.05f);
                SetTotalCoinText();
                SetLevelCoinText();
            }

            Invoke(nameof(OpenNextButton), 1f);

            // ReSharper disable once IteratorNeverReturns
        }

        private void OpenNextButton()
        {
            if (!_nextButton.gameObject.activeSelf) _nextButton.gameObject.SetActive(true);
        }

        private void SetLevelCoinText()
        {
            _levelCoinText.text = "LEVEL COIN " + _levelModel.GetLevelCoin();
        }

        private void SetTotalCoinText()
        {
            _totalCoinText.text = "TOTAL COIN " + _levelModel.GetTotalCoin();
        }
    }
}
