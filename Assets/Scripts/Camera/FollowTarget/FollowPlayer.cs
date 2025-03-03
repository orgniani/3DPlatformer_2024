using System.Collections;
using UnityEngine;

namespace Camera.FollowTarget
{
    public class FollowPlayer : MonoBehaviour
    {
        private float _currentX = 0f;
        private float _currentY = 0f;
        private Transform _target;

        private Coroutine _followCoroutine;

        public FollowPlayerModel Model { get; set; }

        public void StartFollowingTarget(Transform target)
        {
            _target = target;

            if (_followCoroutine != null)
                StopCoroutine(_followCoroutine);

            _followCoroutine = StartCoroutine(FollowTargetCoroutine());
        }

        private IEnumerator FollowTargetCoroutine()
        {
            while (_target)
            {
                Quaternion rotation = Quaternion.Euler(_currentY, _currentX, 0);

                Vector3 offset = Vector3.up * Model.OffsetUp;

                Vector3 negDistance = new Vector3(0.0f, 0.0f, -Model.Distance);
                Vector3 position = rotation * negDistance + _target.position + offset;

                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Model.RotationSpeed * Time.deltaTime);
                transform.position = Vector3.Lerp(transform.position, position, Model.Speed * Time.deltaTime);

                yield return new WaitForFixedUpdate();
            }

            _followCoroutine = null;
        }

        public void SetInputRotation(Vector2 input)
        {
            _currentX += input.x * Model.Sensitivity * Time.deltaTime;
            _currentY -= input.y * Model.Sensitivity * Time.deltaTime;
            _currentY = Mathf.Clamp(_currentY, Model.MinVerticalAngle, Model.MaxVerticalAngle);
        }
    }
}
