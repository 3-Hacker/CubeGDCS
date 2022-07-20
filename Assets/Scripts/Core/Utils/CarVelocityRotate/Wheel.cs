using UnityEngine;

namespace Core.Utils.CarVelocityRotate
{
    public class Wheel : MonoBehaviour
    {
        //[SerializeField] private CarDamage _carDamage;
        public float maxDown;
        public float maxUp;
        public float wheelRadius;
        private Vector3 _awakeLocalPos;

        private void Awake()
        {
            // _carDamage = GetComponentInParent<CarDamage>();
        }

        private void Start()
        {
            _awakeLocalPos = transform.localPosition;
        }

        private void Update()
        {
            // transform.localRotation *= Quaternion.AngleAxis(_car.currentSpeed / wheelRadius, Vector3.right);
            //  if (_carDamage.GetCrash()) return;
            var wheelOrigin = transform.parent.TransformPoint(_awakeLocalPos);
            var ray = new Ray(wheelOrigin + Vector3.up * maxUp, Vector3.down);
            var target = maxDown;

            if (Physics.Raycast(ray, out var info, Mathf.Abs(maxDown) + wheelRadius + maxUp,
                    1 << LayerMask.NameToLayer("Road")))
            {
                Debug.DrawLine(ray.origin, info.point, Color.magenta);
                var diffY = info.point.y - wheelOrigin.y + wheelRadius;
                //diffY -= wheelRadius;
                diffY = Mathf.Clamp(diffY, maxDown, maxUp);
                target = diffY;
            }
            else
            {
                Debug.DrawLine(ray.origin, ray.origin + Vector3.down * (Mathf.Abs(maxDown) + wheelRadius), Color.cyan);
            }

            if (transform.localPosition.y < target)
            {
                transform.localPosition = _awakeLocalPos + Vector3.up * target;
            }
            else
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, _awakeLocalPos + Vector3.up * target,
                    Time.deltaTime * 5.5f);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, wheelRadius);
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + Vector3.up * maxUp);
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, transform.position + Vector3.up * maxDown);
        }
    }
}
