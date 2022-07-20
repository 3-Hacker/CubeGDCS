using UnityEngine;

namespace Core.Utils
{
    public class VelocityBasedCarAnimation : MonoBehaviour
    {
        public float springConstant;
        public float dragRatio;
        public float pitchMultiplier = 1f;
        public float rollMultiplier = 1f;
        private Vector3 _virtualBall;

        private Vector3 _virtualBallVelocity;

        private Vector3 _virtualBallPrevVelocity;


        private void OnEnable()
        {
            _virtualBall = transform.position;
        }

        private void FixedUpdate()
        {
            var force = (_virtualBall - transform.position);
            var x = force.magnitude - 0;
            force = force.normalized * (-1 * springConstant * x);
            _virtualBallVelocity += force * Time.fixedDeltaTime;
            _virtualBall += _virtualBallVelocity * Time.fixedDeltaTime;
            _virtualBallVelocity -= _virtualBallVelocity * dragRatio * Time.fixedDeltaTime;

            var ballAcc = (_virtualBallVelocity - _virtualBallPrevVelocity) / Time.fixedDeltaTime;

            //var ballToCenter = (_virtualBall - transform.position).WithY(0);
            var local = transform.InverseTransformDirection(ballAcc);


            var targetRot =
                Quaternion.LookRotation(transform.parent.forward)
                * Quaternion.AngleAxis(local.z * pitchMultiplier, Vector3.right)
                * Quaternion.AngleAxis(local.x * rollMultiplier, Vector3.forward);


            transform.rotation =
                Quaternion.RotateTowards(
                    transform.rotation,
                    targetRot,
                    Time.fixedDeltaTime * 180f
                );

            _virtualBallPrevVelocity = _virtualBallVelocity;
        }


        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_virtualBall, 1f);
        }
    }
}
