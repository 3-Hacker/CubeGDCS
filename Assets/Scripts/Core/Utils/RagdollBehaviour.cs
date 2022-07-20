using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core.Utils
{
    public class RagdollBehaviour : MonoBehaviour
    {
        private Animator _anim;
        private List<Collider> _ragdollColliders;
        private List<Rigidbody> _ragdollRigidbodies;
        private bool toggle;
        private bool ragdollOn;

        private void Awake()
        {
            _anim = GetComponent<Animator>();
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

        public void RagdollOff()
        {
            this.toggle = !this.toggle;

            ragdollOn = false;

            foreach (var ragdollCollider in _ragdollColliders)
            {
                ragdollCollider.tag = "PlayerBodyPart";
                ragdollCollider.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
            }

            foreach (var ragdollRigidbody in _ragdollRigidbodies)
            {
                ragdollRigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
                ragdollRigidbody.isKinematic = true;
                ragdollRigidbody.useGravity = false;
            }

            _anim.enabled = true;
        }

        public void RagdollOn()
        {
            this.toggle = !this.toggle;

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
        }

        public void Force()
        {
            RagdollOn();
            _anim.GetBoneTransform(HumanBodyBones.Hips).GetComponent<Rigidbody>()
                .AddForce(-Vector3.forward * 75f + Vector3.up * 50f, ForceMode.Impulse);
        }


        public List<Rigidbody> GetRagdollRigidbodies()
        {
            return _ragdollRigidbodies;
        }
    }
}
