using UnityEngine;

namespace Player.Rotation
{
    public class PlayerRotation : MonoBehaviour
    {
        //TODO: Review script
        private Rigidbody rigidBody;

        public RotationModel Model { get; set; }

        private void Reset()
        {
            rigidBody = GetComponent<Rigidbody>();
        }

        private void Awake()
        {
            rigidBody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            RotateCharacter();
        }

        private void RotateCharacter()
        {
            Vector3 velocity = rigidBody.velocity;
            velocity.y = 0;

            if (velocity.magnitude < Model.MinimumSpeedForRotation)
                return;

            float rotationAngle = Vector3.SignedAngle(transform.forward, velocity, Vector3.up);
            transform.Rotate(Vector3.up, rotationAngle * Model.RotationSpeed * Time.deltaTime);
        }
    }

}