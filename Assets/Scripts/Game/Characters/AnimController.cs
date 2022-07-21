using System;
using Core.Utils;
using UnityEngine;

namespace Game.Characters
{
    public class AnimController : MonoBehaviour
    {
        [SerializeField] private Animator _mainAnimator;

        private static readonly int Idle = Animator.StringToHash("idle");
        private static readonly int Run = Animator.StringToHash("run");
        private static readonly int Victory = Animator.StringToHash("victory");

        private void Start()
        {
            ReferenceControl();
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
