using UnityEngine;

namespace Game.Level
{
    public class SceneLoader : MonoBehaviour
    {
        private void Start()
        {
            LoadScene();
        }

        private void LoadScene()
        {
            GameData.LoadCurrentScene();
        }

        // [Button(nameof(CustomSet))] public bool ButtonCustomSetField;
        public void CustomSet(int value)
        {
            GameData.LevelIndex = value;
            GameData.RandomIndex = 0;
        }
    }
}
