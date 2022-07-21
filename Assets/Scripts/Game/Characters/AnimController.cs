using System;
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
        }

        private void OnDisable()
        {
            _gameSignals.GameStart.RemoveListener(OnGameStart);
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
        }
    }
}
