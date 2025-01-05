using UnityEngine;

namespace Camera.FollowTarget
{
    public class FollowPlayer : MonoBehaviour
    {
        [SerializeField] private Transform target;

        private float _currentX = 0f;
        private float _currentY = 0f;

        public FollowPlayerModel Model { get; set; }

        private void Awake()
        {
            if (!target)
            {
                Debug.LogError($"{name}: {nameof(target)} is null!" +
                               $"\nDisabling object to avoid errors.");
                enabled = false;
                return;
            }
        }

        private void FixedUpdate()
        {
            Quaternion rotation = Quaternion.Euler(_currentY, _currentX, 0); //TODO: Research quaternions

            Vector3 offset = Vector3.up * Model.OffsetUp;

            Vector3 negDistance = new Vector3(0.0f, 0.0f, -Model.Distance);
            Vector3 position = rotation * negDistance + target.position + offset;

            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Model.RotationSpeed * Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, position, Model.Speed * Time.deltaTime); //TODO: Research lerp
        }

        public void SetInputRotation(Vector2 input)
        {
            _currentX += input.x * Model.Sensitivity * Time.deltaTime;
            _currentY -= input.y * Model.Sensitivity * Time.deltaTime;
            _currentY = Mathf.Clamp(_currentY, Model.MinVerticalAngle, Model.MaxVerticalAngle);
        }
    }
}
