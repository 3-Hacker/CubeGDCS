using System;
using Game.Model.LevelModel;
using Game.Root;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Level
{
    public class LevelLoader : MonoBehaviour
    {
        private ILevelModel _levelModel;

        private void Start()
        {
            _levelModel = GameInstaller.Instance.LevelModel;
        }

        public void RestartScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void NextLevel()
        {
            _levelModel.SetTotalCoin();
            GameData.LevelIndex++;
            GameData.LoadCurrentScene();
        }
    }
}
