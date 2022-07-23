using System.Collections;
using Core.Utils;
using Game.Root;
using Game.Signals;
using UnityEngine;

namespace Game.Characters
{
    public class AnimController : MonoBehaviour
    {
        [SerializeField] private Animator _mainAnimator;
        private GameSignals _gameSignals;

        private static readonly int Idle = Animator.StringToHash("idle");
        private static readonly int Run = Animator.StringToHash("run");
        private static readonly int Victory = Animator.StringToHash("victory");

        private void Awake()
        {
            SetReference();
        }

        private void Start()
        {
            ReferenceControl();
        }

        private void SetReference()
        {
            _gameSignals = GameInstaller.Instance.GameSignal;
        }

        private void OnEnable()
        {
            _gameSignals.GameStart.AddListener(OnGameStart);
            _gameSignals.LevelFinish.AddListener(OnLevelFinish);
        }

        private void OnDisable()
        {
            _gameSignals.GameStart.RemoveListener(OnGameStart);
            _gameSignals.LevelFinish.RemoveListener(OnLevelFinish);
        }

        private void OnLevelFinish()
        {
            IdleAnim();
            Invoke(nameof(RotateBack), 0.5f);
        }

        private void OnGameStart()
        {
            RunAnim();
        }

        private void ReferenceControl()
        {
            if (_mainAnimator == null) _mainAnimator = GetComponentInChildren<Animator>();
        }

        [Button(nameof(IdleAnim))] public bool IdleAnimButton;

        public void IdleAnim()
        {
            _mainAnimator.ResetTrigger(Run);
            _mainAnimator.SetTrigger(Idle);
        }

        [Button(nameof(RunAnim))] public bool RunAnimButton;

        public void RunAnim()
        {
            _mainAnimator.ResetTrigger(Idle);
            _mainAnimator.SetTrigger(Run);
        }

        [Button(nameof(VictoryAnim))] public bool VictoryAnimButton;

        public void VictoryAnim()
        {
            _mainAnimator.SetTrigger(Victory);
            _gameSignals.PlayerVictory.Dispatch();
        }

        [Button(nameof(RotateBack))] public bool RotateBackButton;

        public void RotateBack()
        {
            StartCoroutine(RotateCoroutine());
        }

        private IEnumerator RotateCoroutine()
        {
            var time = 1f;

            while (true)
            {
                if (time > 0f)
                {
                    time -= Time.deltaTime;

                    var rot = transform.rotation;
                    rot.y = 180f;
                    transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * (1f / time));
                }
                else
                {
                    VictoryAnim();
                    yield break;
                }

                yield return new WaitForEndOfFrame();
            }
        }
    }
}
