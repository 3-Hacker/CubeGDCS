using UnityEngine;

namespace Core.Utils
{
    public class TimeManager : MonoSingleton<TimeManager>
    {
        #region Variables

        public float slowdownMultiply = 0.05f;
        public float slowdonwLength = 1f;
        [Range(.01f, 1f)] public float defaultTimeSensivite = .1f;

        private bool isDefault;
        private bool isToggle;

        #endregion

        #region Normal Methods

        private void Update()
        {
            if (isDefault)
            {
                Time.timeScale += (1f / (slowdonwLength * defaultTimeSensivite) * Time.unscaledDeltaTime);
                Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
            }
        }

        #endregion

        #region Custom Methods

        public void SlowMo()
        {
            this.isToggle = !this.isToggle;
            isDefault = false;
            Time.timeScale = slowdownMultiply;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
        }


        public void FastMo()
        {
            this.isToggle = !this.isToggle;
            isDefault = true;
        }

        #endregion
    }
}
