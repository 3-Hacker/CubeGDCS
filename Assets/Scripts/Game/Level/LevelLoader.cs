using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Level
{
    public class LevelLoader : MonoBehaviour
    {
        public void RestartScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void NextLevel()
        {
            GameData.LevelIndex++;
            GameData.LoadCurrentScene();
        }
    }
}
