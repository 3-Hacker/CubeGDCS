using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Level
{
    public static class GameData
    {
        private static readonly int TotalLevelCount = 5;
        public static int OutRipTrick { get; set; }

        public static int VisualLevelIndex => LevelIndex + 1;

        public static int LevelIndex
        {
            get => PlayerPrefs.GetInt("LEVEL", 0); //ES3.Load<int>("LEVEL", 0);
            set => PlayerPrefs.SetInt("LEVEL", value); //ES3.Save<int>("LEVEL", value);
        }

        public static int RandomIndex
        {
            get => PlayerPrefs.GetInt("RandomIndex", 0); //ES3.Load<int>("RandomIndex", 0);
            set
            {
                Debug.Log("RandomIndex ->" + value);
                PlayerPrefs.SetInt("RandomIndex", value); // ES3.Save<int>("RandomIndex", value);
            }
        }

        public static readonly float Sensivity = 750f;

        public static void LoadCurrentScene()
        {
            var levelResult = (LevelIndex) % (TotalLevelCount);
            SceneManager.LoadScene(levelResult + 1);
        }
    }
}
