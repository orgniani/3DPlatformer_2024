using UnityEngine;

namespace Player.Rotation
{
    [RequireComponent((typeof(Rigidbody)))]
    public class PlayerRotation : MonoBehaviour
    {
        private Rigidbody _rigidBody;

        public RotationModel Model { get; set; }

        private void Reset()
        {
            _rigidBody = GetComponent<Rigidbody>();
        }

        private void Awake()
        {
            _rigidBody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            RotateCharacter();
        }

        private void RotateCharacter()
        {
            Vector3 velocity = _rigidBody.velocity;
            velocity.y = 0;

            if (velocity.magnitude < Model.MinimumSpeedForRotation)
                return;

            float rotationAngle = Vector3.SignedAngle(transform.forward, velocity, Vector3.up);
            transform.Rotate(Vector3.up, rotationAngle * Model.RotationSpeed * Time.deltaTime);
        }
    }
}