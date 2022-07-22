using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core.Utils
{
    public class RagdollBehaviour : MonoBehaviour
    {
        [SerializeField] private Animator _anim;
        private Collider _mainCollider;
        private List<Collider> _ragdollColliders;
        private List<Rigidbody> _ragdollRigidbodies;
        private bool ragdollOn;

        private void Awake()
        {
            if (_anim == null) _anim = GetComponentInChildren<Animator>();
            _mainCollider = GetComponent<Collider>();
            _ragdollColliders = GetComponentsInChildren<Collider>().ToList();
            _ragdollRigidbodies = GetComponentsInChildren<Rigidbody>().ToList();
        }

        private void Start()
        {
            RagdollOff();
        }

        public bool GetRagdollState()
        {
            return ragdollOn;
        }

        [Button(nameof(RagdollOff))] public bool RagdollOffButton;


        public void RagdollOff()
        {
            ragdollOn = false;


            foreach (var ragdollRigidbody in _ragdollRigidbodies)
            {
                ragdollRigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
                ragdollRigidbody.isKinematic = true;
                ragdollRigidbody.useGravity = false;
            }

            _anim.enabled = true;
        }

        [Button(nameof(RagdollOn))] public bool RagdollOnButton;


        public void RagdollOn()
        {
            ragdollOn = true;


            foreach (var ragdollCollider in _ragdollColliders)
            {
                ragdollCollider.enabled = true;
            }

            foreach (var ragdollRigidbody in _ragdollRigidbodies)
            {
                ragdollRigidbody.isKinematic = false;
                ragdollRigidbody.useGravity = true;
                ragdollRigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
            }

            _anim.enabled = false;
            _mainCollider.isTrigger = false;
        }

        [Button(nameof(Force))] public bool ForceButton;

        public void Force()
        {
            RagdollOn();
            _anim.GetBoneTransform(HumanBodyBones.Hips).GetComponent<Rigidbody>()
                .AddForce(Vector3.forward * 75f + Vector3.up * 50f, ForceMode.Impulse);
        }


        public List<Rigidbody> GetRagdollRigidbodies()
        {
            return _ragdollRigidbodies;
        }
    }
}
